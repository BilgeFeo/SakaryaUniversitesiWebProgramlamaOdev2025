using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;
using WebProgramlamaOdev.ViewModels;

namespace WebProgramlamaOdev.Controllers
{
    [Authorize(Roles = "Trainer")]
    public class TrainerAppointmentManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<ApplicationUser> _repository;

        // İstediğin Dependency Injection yapısı
        public TrainerAppointmentManagementController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IRepository<ApplicationUser> repository)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _repository = repository;
        }

        // ============================================
        // YARDIMCI METOD: Giriş Yapan Antrenörü Getir
        // ============================================
        private async Task<Trainer?> GetCurrentTrainerAsync()
        {
            var userIdString = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdString)) return null;

            var user = await _repository.GetByIdAsync(userIdString); // ID tipine göre int.Parse gerekebilir
            if (user == null) return null;

            return await _unitOfWork.Trainers.GetByUserIdWithDetailsAsync(user.Id);
        }

        // ============================================
        // SHOW APPOINTMENTS - Antrenörün Randevularını Listele
        // ============================================
        public async Task<IActionResult> ShowAppointments(string? status = null)
        {
            try
            {
                var trainer = await GetCurrentTrainerAsync();
                if (trainer == null)
                {
                    TempData["ErrorMessage"] = "Antrenör bilgileri bulunamadı.";
                    return RedirectToAction("Index", "TrainerHome");
                }

                ViewBag.TrainerName = $"{trainer.User.FirstName} {trainer.User.LastName}";
                ViewBag.GymName = trainer.Gym?.Name ?? "Belirtilmemiş";

                // 1. Sadece bu antrenöre ait randevuları çek
                var appointments = await _unitOfWork.Appointments.GetByTrainerIdAsync(trainer.Id);

                // 2. Eğer status filtresi varsa memory'de filtrele
                if (!string.IsNullOrEmpty(status))
                {
                    appointments = appointments.Where(a => a.Status == status);
                }

                // 3. ViewModel'e dönüştür
                var viewModel = appointments.Select(a => new AppointmentListViewModel
                {
                    Id = a.Id,

                    // Member Bilgileri
                    MemberId = a.MemberId,
                    MemberName = a.Member?.User != null
                        ? $"{a.Member.User.FirstName} {a.Member.User.LastName}".Trim()
                        : "Bilinmiyor",
                    MemberEmail = a.Member?.User?.Email ?? "Belirtilmemiş",
                    MemberPhone = a.Member?.User?.PhoneNumber ?? "Belirtilmemiş",

                    // Trainer Bilgileri (Kendisi)
                    TrainerId = a.TrainerId,
                    TrainerName = $"{trainer.User.FirstName} {trainer.User.LastName}",
                    TrainerSpecialization = trainer.Specialization ?? "Belirtilmemiş",

                    // Service Bilgileri
                    ServiceTypeId = a.ServiceTypeId,
                    ServiceName = a.ServiceType?.Name ?? "Bilinmiyor",
                    ServicePrice = a.ServiceType?.Price ?? 0,

                    // Randevu Bilgileri
                    AppointmentDate = a.AppointmentDate,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    AppointmentDateTime = a.AppointmentDateTime,

                    // Durum Bilgileri
                    Status = a.Status,
                    IsConfirmed = a.IsConfirmed,
                    TotalPrice = a.TotalPrice,

                    CreatedDate = a.CreatedDate

                }).ToList();

                // İstatistikler (Sadece bu antrenör için)
                // Not: appointments değişkeni filtrelenmiş olabilir, o yüzden tümünü tekrar saymak gerekebilir
                // Ancak performans için viewmodel üzerinden sayıyoruz (Filtreli ekrandaysa sadece filtreli sayıları görür)
                ViewBag.TotalAppointments = viewModel.Count;
                ViewBag.PendingCount = viewModel.Count(a => a.Status == "Pending");
                ViewBag.ConfirmedCount = viewModel.Count(a => a.Status == "Confirmed");
                ViewBag.CancelledCount = viewModel.Count(a => a.Status == "Cancelled");
                ViewBag.CompletedCount = viewModel.Count(a => a.Status == "Completed");
                ViewBag.CurrentStatus = status;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Randevular yüklenirken hata: " + ex.Message;
                return View(new List<AppointmentListViewModel>());
            }
        }

        // ============================================
        // CONFIRM APPOINTMENT - Randevu Onayla
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmAppointment(int id)
        {
            try
            {
                var trainer = await GetCurrentTrainerAsync();
                if (trainer == null) return RedirectToAction("Login", "Account");

                var appointment = await _unitOfWork.Appointments.GetWithDetailsAsync(id);

                if (appointment == null)
                {
                    TempData["ErrorMessage"] = "Randevu bulunamadı.";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                // GÜVENLİK: Randevu bu antrenöre mi ait?
                if (appointment.TrainerId != trainer.Id)
                {
                    TempData["ErrorMessage"] = "Size ait olmayan bir randevuyu onaylayamazsınız!";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                if (appointment.Status != "Pending")
                {
                    TempData["ErrorMessage"] = "Sadece beklemedeki randevular onaylanabilir.";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                // Çakışma kontrolü
                var hasConflict = await _unitOfWork.Appointments.HasConflictAsync(
                    trainer.Id,
                    appointment.AppointmentDate,
                    appointment.StartTime,
                    appointment.EndTime,
                    appointment.Id
                );

                if (hasConflict)
                {
                    TempData["ErrorMessage"] = "Bu saatte başka bir randevunuz var!";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                var result = await _unitOfWork.Appointments.ConfirmAppointmentAsync(id);
                if (result)
                {
                    await _unitOfWork.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Randevu başarıyla onaylandı.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Onaylama işlemi başarısız.";
                }

                return RedirectToAction(nameof(ShowAppointments));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Hata: " + ex.Message;
                return RedirectToAction(nameof(ShowAppointments));
            }
        }

        // ============================================
        // CANCEL APPOINTMENT - Randevu İptal
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAppointment(int id, string? reason = null)
        {
            try
            {
                var trainer = await GetCurrentTrainerAsync();
                if (trainer == null) return RedirectToAction("Login", "Account");

                var appointment = await _unitOfWork.Appointments.GetWithDetailsAsync(id);

                if (appointment == null || appointment.TrainerId != trainer.Id)
                {
                    TempData["ErrorMessage"] = "Randevu bulunamadı veya yetkiniz yok.";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                if (appointment.Status == "Completed")
                {
                    TempData["ErrorMessage"] = "Tamamlanmış randevular iptal edilemez.";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                var result = await _unitOfWork.Appointments.CancelAppointmentAsync(id);
                if (result)
                {
                    await _unitOfWork.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Randevu iptal edildi." + (reason != null ? $" Neden: {reason}" : "");
                }
                else
                {
                    TempData["ErrorMessage"] = "İptal işlemi başarısız.";
                }

                return RedirectToAction(nameof(ShowAppointments));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Hata: " + ex.Message;
                return RedirectToAction(nameof(ShowAppointments));
            }
        }

        // ============================================
        // DELETE APPOINTMENT - Randevu Sil
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                var trainer = await GetCurrentTrainerAsync();
                if (trainer == null) return RedirectToAction("Login", "Account");

                var appointment = await _unitOfWork.Appointments.GetWithDetailsAsync(id);

                if (appointment == null || appointment.TrainerId != trainer.Id)
                {
                    TempData["ErrorMessage"] = "Randevu bulunamadı veya yetkiniz yok.";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                if (appointment.Status == "Confirmed" || appointment.Status == "Completed")
                {
                    TempData["ErrorMessage"] = "Onaylanmış veya tamamlanmış randevular silinemez. Önce iptal edin.";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                await _unitOfWork.Appointments.DeleteAsync(appointment);
                await _unitOfWork.SaveChangesAsync();

                TempData["SuccessMessage"] = "Randevu kaydı silindi.";
                return RedirectToAction(nameof(ShowAppointments));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Hata: " + ex.Message;
                return RedirectToAction(nameof(ShowAppointments));
            }
        }

        // ============================================
        // APPOINTMENT DETAILS - Detay Görüntüle
        // ============================================
        public async Task<IActionResult> AppointmentDetails(int id)
        {
            try
            {
                var trainer = await GetCurrentTrainerAsync();
                var appointment = await _unitOfWork.Appointments.GetWithDetailsAsync(id);

                if (appointment == null || appointment.TrainerId != trainer.Id)
                {
                    TempData["ErrorMessage"] = "Randevu bulunamadı veya görüntüleme yetkiniz yok.";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                // ViewModel map işlemi (Admin ile aynı)
                var viewModel = new AppointmentListViewModel
                {
                    Id = appointment.Id,
                    MemberId = appointment.MemberId,
                    MemberName = appointment.Member?.User != null ? $"{appointment.Member.User.FirstName} {appointment.Member.User.LastName}" : "Bilinmiyor",
                    MemberEmail = appointment.Member?.User?.Email ?? "-",
                    MemberPhone = appointment.Member?.User?.PhoneNumber ?? "-",
                    TrainerId = appointment.TrainerId,
                    TrainerName = $"{trainer.User.FirstName} {trainer.User.LastName}",
                    TrainerSpecialization = trainer.Specialization,
                    ServiceTypeId = appointment.ServiceTypeId,
                    ServiceName = appointment.ServiceType?.Name ?? "-",
                    ServicePrice = appointment.ServiceType?.Price ?? 0,
                    AppointmentDate = appointment.AppointmentDate,
                    StartTime = appointment.StartTime,
                    EndTime = appointment.EndTime,
                    AppointmentDateTime = appointment.AppointmentDateTime,
                    Status = appointment.Status,
                    IsConfirmed = appointment.IsConfirmed,
                    TotalPrice = appointment.TotalPrice,
                    CreatedDate = appointment.CreatedDate
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Detaylar yüklenirken hata: " + ex.Message;
                return RedirectToAction(nameof(ShowAppointments));
            }
        }
    }
}