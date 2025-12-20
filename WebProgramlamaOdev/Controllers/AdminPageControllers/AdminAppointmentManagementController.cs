using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebProgramlamaOdev.Repositories.Interfaces;
using WebProgramlamaOdev.ViewModels;

namespace WebProgramlamaOdev.Controllers.AdminPageControllers
{
    [Authorize(Roles = "Gym")]
    public class AdminAppointmentManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminAppointmentManagementController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ============================================
        // SHOW APPOINTMENTS - Randevuları Listele
        // ============================================
        public async Task<IActionResult> ShowAppointments(string? status = null)
        {
            try
            {
                IEnumerable<Models.Appointment> appointments;

                // Status filtresine göre getir
                if (!string.IsNullOrEmpty(status))
                {
                    appointments = await _unitOfWork.Appointments.GetByStatusAsync(status);
                }
                else
                {
                    appointments = await _unitOfWork.Appointments.GetAllWithDetailsAsync();
                }

                // ViewModel'e dönüştür
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
                    
                    // Trainer Bilgileri
                    TrainerId = a.TrainerId,
                    TrainerName = a.Trainer?.User != null
                        ? $"{a.Trainer.User.FirstName} {a.Trainer.User.LastName}".Trim()
                        : "Bilinmiyor",
                    TrainerSpecialization = a.Trainer?.Specialization ?? "Belirtilmemiş",
                    
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
                    
                    // Ek Bilgiler
                    CreatedDate = a.CreatedDate
                    
                }).ToList();

                // İstatistikler için ViewBag
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
                TempData["ErrorMessage"] = "Randevular yüklenirken bir hata oluştu: " + ex.Message;
                return View(new List<AppointmentListViewModel>());
            }
        }

        // ============================================
        // CONFIRM APPOINTMENT - Randevu Onayla (POST)
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmAppointment(int id)
        {
            try
            {
                var appointment = await _unitOfWork.Appointments.GetWithDetailsAsync(id);

                if (appointment == null)
                {
                    TempData["ErrorMessage"] = "Randevu bulunamadı.";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                if (appointment.Status != "Pending")
                {
                    TempData["ErrorMessage"] = "Sadece beklemedeki randevular onaylanabilir.";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                // Çakışma kontrolü
                var hasConflict = await _unitOfWork.Appointments.HasConflictAsync(
                    appointment.TrainerId,
                    appointment.AppointmentDate,
                    appointment.StartTime,
                    appointment.EndTime,
                    appointment.Id
                );

                if (hasConflict)
                {
                    TempData["ErrorMessage"] = "Bu saatte antrenörün başka bir randevusu var!";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                // Randevuyu onayla
                var result = await _unitOfWork.Appointments.ConfirmAppointmentAsync(id);

                if (!result)
                {
                    TempData["ErrorMessage"] = "Randevu onaylanamadı.";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                await _unitOfWork.SaveChangesAsync();

                var memberName = appointment.Member?.User != null
                    ? $"{appointment.Member.User.FirstName} {appointment.Member.User.LastName}"
                    : "Üye";

                TempData["SuccessMessage"] = $"{memberName} adlı üyenin randevusu başarıyla onaylandı!";

                return RedirectToAction(nameof(ShowAppointments));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Randevu onaylanırken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(ShowAppointments));
            }
        }

        // ============================================
        // CANCEL APPOINTMENT - Randevu İptal Et (POST)
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAppointment(int id, string? reason = null)
        {
            try
            {
                var appointment = await _unitOfWork.Appointments.GetWithDetailsAsync(id);

                if (appointment == null)
                {
                    TempData["ErrorMessage"] = "Randevu bulunamadı.";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                if (appointment.Status == "Cancelled")
                {
                    TempData["ErrorMessage"] = "Randevu zaten iptal edilmiş.";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                if (appointment.Status == "Completed")
                {
                    TempData["ErrorMessage"] = "Tamamlanmış randevular iptal edilemez.";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                // Randevuyu iptal et
                var result = await _unitOfWork.Appointments.CancelAppointmentAsync(id);

                if (!result)
                {
                    TempData["ErrorMessage"] = "Randevu iptal edilemedi.";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                await _unitOfWork.SaveChangesAsync();

                var memberName = appointment.Member?.User != null
                    ? $"{appointment.Member.User.FirstName} {appointment.Member.User.LastName}"
                    : "Üye";

                TempData["SuccessMessage"] = $"{memberName} adlı üyenin randevusu iptal edildi.";
                
                if (!string.IsNullOrEmpty(reason))
                {
                    TempData["SuccessMessage"] += $" Neden: {reason}";
                }

                return RedirectToAction(nameof(ShowAppointments));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Randevu iptal edilirken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(ShowAppointments));
            }
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                var appointment = await _unitOfWork.Appointments.GetWithDetailsAsync(id);

                if (appointment == null)
                {
                    TempData["ErrorMessage"] = "Silinecek randevu bulunamadı.";
                    return RedirectToAction(nameof(ShowAppointments));
                }

               
                if (appointment.Status == "Confirmed" || appointment.Status == "Completed")
                {
                    TempData["ErrorMessage"] = "Onaylanmış veya tamamlanmış randevular silinemez. Önce iptal edin.";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                var memberName = appointment.Member?.User != null
                    ? $"{appointment.Member.User.FirstName} {appointment.Member.User.LastName}"
                    : "Üye";

                var appointmentDate = appointment.AppointmentDate.ToString("dd MMMM yyyy");

                
                await _unitOfWork.Appointments.DeleteAsync(appointment);
                await _unitOfWork.SaveChangesAsync();

                TempData["SuccessMessage"] = $"{memberName} adlı üyenin {appointmentDate} tarihli randevusu silindi.";

                return RedirectToAction(nameof(ShowAppointments));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Randevu silinirken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(ShowAppointments));
            }
        }

        public async Task<IActionResult> AppointmentDetails(int id)
        {
            try
            {
                var appointment = await _unitOfWork.Appointments.GetWithDetailsAsync(id);

                if (appointment == null)
                {
                    TempData["ErrorMessage"] = "Randevu bulunamadı.";
                    return RedirectToAction(nameof(ShowAppointments));
                }

                var viewModel = new AppointmentListViewModel
                {
                    Id = appointment.Id,
                    
                    
                    MemberId = appointment.MemberId,
                    MemberName = appointment.Member?.User != null
                        ? $"{appointment.Member.User.FirstName} {appointment.Member.User.LastName}".Trim()
                        : "Bilinmiyor",
                    MemberEmail = appointment.Member?.User?.Email ?? "Belirtilmemiş",
                    MemberPhone = appointment.Member?.User?.PhoneNumber ?? "Belirtilmemiş",
                    
                    TrainerId = appointment.TrainerId,
                    TrainerName = appointment.Trainer?.User != null
                        ? $"{appointment.Trainer.User.FirstName} {appointment.Trainer.User.LastName}".Trim()
                        : "Bilinmiyor",
                    TrainerSpecialization = appointment.Trainer?.Specialization ?? "Belirtilmemiş",
                    
                    ServiceTypeId = appointment.ServiceTypeId,
                    ServiceName = appointment.ServiceType?.Name ?? "Bilinmiyor",
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
                TempData["ErrorMessage"] = "Randevu detayları yüklenirken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(ShowAppointments));
            }
        }
    }
}
