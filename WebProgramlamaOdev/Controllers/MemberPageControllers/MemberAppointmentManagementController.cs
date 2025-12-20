using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;
using WebProgramlamaOdev.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace WebProgramlamaOdev.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberAppointmentManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<ApplicationUser> _repository;


        public MemberAppointmentManagementController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IRepository<ApplicationUser> repository)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _repository = repository;
        }


        // ============================================
        // YARDIMCI METOD: Giriş Yapan Üyeyi Getir
        // ============================================
        private async Task<Member?> GetCurrentMemberAsync()
        {
            var userIdString = User.FindFirst("UserId")?.Value;
            

            if (string.IsNullOrEmpty(userIdString)) return null;

            // Member tablosundan UserId ile üyeyi buluyoruz
            return await _unitOfWork.Members.GetByUserIdAsync(userIdString);
        }

        // ============================================
        // INDEX - Randevuları Listele
        // ============================================
        public async Task<IActionResult> Index()
        {
            try
            {
                var member = await GetCurrentMemberAsync();
                if (member == null)
                {
                    TempData["ErrorMessage"] = "Üye bilgileri bulunamadı.";
                    return RedirectToAction("Index", "MemberHome");
                }

                // 1. Üyenin randevularını repository'den çek
                // (AppointmentRepository içinde GetByMemberIdAsync metodu include'ları yapıyor)
                var appointments = await _unitOfWork.Appointments.GetByMemberIdAsync(member.Id);

                // 2. ViewModel'e map et
                var viewModel = appointments.Select(a => new AppointmentListViewModel
                {
                    Id = a.Id,

                    // Member (Kendisi)
                    MemberId = a.MemberId,
                    MemberName = $"{member.User?.FirstName} {member.User?.LastName}",
                    MemberEmail = member.User?.Email ?? "-",
                    MemberPhone = member.User?.PhoneNumber ?? "-",

                    // Trainer Bilgileri
                    TrainerId = a.TrainerId,
                    TrainerName = a.Trainer?.User != null
                        ? $"{a.Trainer.User.FirstName} {a.Trainer.User.LastName}"
                        : "Bilinmiyor",
                    TrainerSpecialization = a.Trainer?.Specialization ?? "Belirtilmemiş",

                    // Service Bilgileri
                    ServiceTypeId = a.ServiceTypeId,
                    ServiceName = a.ServiceType?.Name ?? "Hizmet Silinmiş",
                    ServicePrice = a.ServiceType?.Price ?? 0,

                    // Randevu Detayları
                    AppointmentDate = a.AppointmentDate,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    AppointmentDateTime = a.AppointmentDateTime, //

                    // Durum
                    Status = a.Status,
                    IsConfirmed = a.IsConfirmed, //
                    TotalPrice = a.TotalPrice,
                    CreatedDate = a.CreatedDate
                })
                .OrderByDescending(x => x.AppointmentDate) // En yakın tarih en üstte
                .ToList();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Randevular yüklenirken bir hata oluştu: " + ex.Message;
                return View(new List<AppointmentListViewModel>());
            }
        }

        // ============================================
        // CANCEL - Randevu İptal Et (Status -> Cancelled)
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            try
            {
                var member = await GetCurrentMemberAsync();
                if (member == null) return RedirectToAction("Login", "Account");

                var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);

                if (appointment == null || appointment.MemberId != member.Id)
                {
                    TempData["ErrorMessage"] = "Randevu bulunamadı veya işlem yetkiniz yok.";
                    return RedirectToAction(nameof(Index));
                }

                // Tamamlanmış randevular iptal edilemez
                if (appointment.Status == "Completed")
                {
                    TempData["ErrorMessage"] = "Tamamlanmış randevular iptal edilemez.";
                    return RedirectToAction(nameof(Index));
                }

                // Zaten iptal edilmişse uyarı ver
                if (appointment.Status == "Cancelled")
                {
                    TempData["ErrorMessage"] = "Bu randevu zaten iptal edilmiş.";
                    return RedirectToAction(nameof(Index));
                }

                // Durumu güncelle
                appointment.Status = "Cancelled";
                appointment.IsConfirmed = false; // Onayı kaldır

                await _unitOfWork.Appointments.UpdateAsync(appointment);
                await _unitOfWork.SaveChangesAsync();

                TempData["SuccessMessage"] = "Randevunuz başarıyla iptal edildi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "İptal işlemi sırasında hata: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // ============================================
        // DELETE - Randevuyu Sistemden Sil
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                var member = await GetCurrentMemberAsync();
                if (member == null) return RedirectToAction("Login", "Account");

                var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);

                if (appointment == null || appointment.MemberId != member.Id)
                {
                    TempData["ErrorMessage"] = "Randevu bulunamadı veya işlem yetkiniz yok.";
                    return RedirectToAction(nameof(Index));
                }

                // Sadece 'Pending' veya 'Cancelled' olanlar silinebilir.
                // 'Confirmed' olanlar önce iptal edilmelidir.
                if (appointment.Status == "Confirmed" || appointment.Status == "Completed")
                {
                    TempData["ErrorMessage"] = "Onaylanmış randevuları silemezsiniz. Lütfen önce iptal ediniz.";
                    return RedirectToAction(nameof(Index));
                }

                // Veritabanından sil
                await _unitOfWork.Appointments.DeleteAsync(appointment);
                await _unitOfWork.SaveChangesAsync();

                TempData["SuccessMessage"] = "Randevu kaydı başarıyla silindi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Silme işlemi sırasında hata: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }



        // ============================================
        // CREATE (GET) - Randevu Alma Sayfası
        // ============================================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Tüm aktif antrenörleri getir
            var trainers = await _unitOfWork.Trainers.GetAllAsync(); // Repository'de GetAllAsync varsa
                                                                     // Eğer yoksa, basitçe _context.Trainers üzerinden de çekilebilir ama Repository pattern'e sadık kalalım:
                                                                     // Not: UnitOfWork üzerinden özelleşmiş bir metod gerekebilir, şimdilik varsayılanı kullanıyorum.

            // ViewModel'i hazırla
            var viewModel = new CreateAppointmentViewModel
            {
                Trainers = trainers
                    .Where(t => t.IsActive)
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = $"{t.User.FirstName} {t.User.LastName} - {t.Specialization}"
                    }).ToList(),

                // Varsayılan tarih yarın olsun
                AppointmentDate = DateTime.Today.AddDays(1),
                StartTime = new TimeSpan(09, 00, 0) // Sabah 09:00
            };

            return View(viewModel);
        }

        // ============================================
        // AJAX: Antrenöre Göre Hizmetleri Getir
        // ============================================
        [HttpGet]
        public async Task<IActionResult> GetServicesByTrainer(int trainerId)
        {
            var servicesByTrainer = await _unitOfWork.ServicesByTrainers.GetByTrainerIdAsync(trainerId);

            var services = servicesByTrainer.Select(s => new
            {
                id = s.ServiceTypeId,
                name = s.serviceType.Name,
                duration = s.serviceType.Duration, // dakika
                price = s.serviceType.Price,
                description = s.serviceType.Description
            });

            return Json(services);
        }

        // ============================================
        // CREATE (POST) - Randevuyu Kaydet
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Hata durumunda dropdown'ı tekrar doldur
                var trainers = await _unitOfWork.Trainers.GetAllAsync();
                model.Trainers = trainers.Where(t => t.IsActive)
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = $"{t.User.FirstName} {t.User.LastName}"
                    }).ToList();
                return View(model);
            }

            try
            {
                var member = await GetCurrentMemberAsync();
                if (member == null) return RedirectToAction("Login", "Account");

                // 1. Hizmet Detaylarını Al (Süre ve Fiyat Hesaplaması İçin)
                var serviceType = await _unitOfWork.ServiceTypes.GetByIdAsync(model.ServiceTypeId);
                if (serviceType == null)
                {
                    TempData["ErrorMessage"] = "Seçilen hizmet bulunamadı.";
                    return RedirectToAction(nameof(Create));
                }

                // 2. Bitiş Saatini Hesapla
                TimeSpan endTime = model.StartTime.Add(TimeSpan.FromMinutes(serviceType.Duration));

                // 3. Çalışma Saatleri Kontrolü (Örn: 09:00 - 22:00 arası)
                TimeSpan gymOpen = new TimeSpan(09, 00, 0);
                TimeSpan gymClose = new TimeSpan(22, 00, 0);

                if (model.StartTime < gymOpen || endTime > gymClose)
                {
                    TempData["ErrorMessage"] = $"Randevu saatleri çalışma saatleri ({gymOpen:hh\\:mm} - {gymClose:hh\\:mm}) dışında olamaz.";
                    return RedirectToAction(nameof(Create));
                }

                // 4. ÇAKIŞMA KONTROLÜ 1: Mevcut Randevular
                // AppointmentRepository'deki HasConflictAsync metodu
                bool hasAppointmentConflict = await _unitOfWork.Appointments.HasConflictAsync(
                    model.TrainerId,
                    model.AppointmentDate,
                    model.StartTime,
                    endTime
                );

                if (hasAppointmentConflict)
                {
                    TempData["ErrorMessage"] = "Seçilen saat aralığında antrenörün başka bir randevusu mevcut.";
                    return RedirectToAction(nameof(Create));
                }

                // 5. ÇAKIŞMA KONTROLÜ 2: TrainerAvailability Tablosu
                // İsteğin üzerine: Availability tablosunda bir kayıt varsa randevu oluşturulmayacak.
                // (Repository'ye manuel sorgu atıyoruz)
                var availabilityConflict = await _unitOfWork.Context.Set<TrainerAvailability>()
                    .AnyAsync(ta =>
                        ta.TrainerId == model.TrainerId &&
                        ta.EventDate.Date == model.AppointmentDate.Date &&
                        ta.IsActive == true &&
                        // Zaman aralığı çakışması mantığı:
                        (ta.StartTime < endTime && ta.EndTime > model.StartTime)
                    );

                if (availabilityConflict)
                {
                    TempData["ErrorMessage"] = "Antrenör seçilen saat aralığında müsait değil (İzinli veya Dolu).";
                    return RedirectToAction(nameof(Create));
                }

                // 6. Randevu Oluştur
                var appointment = new Appointment
                {
                    MemberId = member.Id,
                    TrainerId = model.TrainerId,
                    ServiceTypeId = model.ServiceTypeId,
                    AppointmentDate = model.AppointmentDate,
                    StartTime = model.StartTime,
                    EndTime = endTime,
                    TotalPrice = serviceType.Price,
                    Status = "Pending", // İlk başta onay bekliyor
                    IsConfirmed = false,
                    CreatedDate = DateTime.Now
                };

                await _unitOfWork.Appointments.AddAsync(appointment);
                await _unitOfWork.SaveChangesAsync();

                // NOT: "TrainerAvaliblity modeli doldurulacak" isteğinize istinaden;
                // Normalde Appointment tablosu doluluk takibi için yeterlidir. 
                // Eğer burada ayrıca Availability tablosuna da kayıt atmak isterseniz bu veri tekrarı olur (Redundant).
                // Ancak sisteminizde "Availability" tablosu "Meşguliyet" olarak kullanılıyorsa, aşağıyı açabilirsiniz:
                /*
                var busySlot = new TrainerAvailability
                {
                    TrainerId = model.TrainerId,
                    EventDate = model.AppointmentDate,
                    StartTime = model.StartTime,
                    EndTime = endTime,
                    IsActive = true // Bu saat aralığı artık dolu
                };
                await _unitOfWork.Context.Set<TrainerAvailability>().AddAsync(busySlot);
                await _unitOfWork.SaveChangesAsync();
                */

                TempData["SuccessMessage"] = "Randevu talebiniz başarıyla oluşturuldu! Antrenör onayı bekleniyor.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Randevu oluşturulurken hata: " + ex.Message;
                return RedirectToAction(nameof(Create));
            }
        }

    }
}