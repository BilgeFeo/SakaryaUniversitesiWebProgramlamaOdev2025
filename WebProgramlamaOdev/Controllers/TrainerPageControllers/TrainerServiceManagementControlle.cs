using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;
using WebProgramlamaOdev.ViewModels;

namespace WebProgramlamaOdev.Controllers
{
    [Authorize(Roles = "Trainer")]
    public class TrainerServiceManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<ApplicationUser> _repository;


        public TrainerServiceManagementController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IRepository<ApplicationUser> repository)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _repository = repository;
        }


        // ============================================
        // HİZMETLERİ GÖSTER + YENİ HİZMET EKLE
        // ============================================
        public async Task<IActionResult> ShowServices()
        {
            try
            {
                var userIdString = User.FindFirst("UserId")?.Value;

                var user = await _repository.GetByIdAsync(userIdString);
                if (user == null) return RedirectToAction("Login", "Account");

                var trainer = await _unitOfWork.Trainers.GetByUserIdWithDetailsAsync(user.Id);
                if (trainer == null)
                {
                    TempData["ErrorMessage"] = "Antrenör bilgileri bulunamadı.";
                    return RedirectToAction("Index", "TrainerHome");
                }

                // ViewBag için
                ViewBag.TrainerName = $"{user.FirstName} {user.LastName}";
                ViewBag.GymName = trainer.Gym?.Name ?? "Belirtilmemiş";

                // ============================================
                // 1. MEVCUT HİZMETLERİ AL
                // ============================================
                var servicesByTrainer = await _unitOfWork.ServicesByTrainers.GetByTrainerIdAsync(trainer.Id);

                var currentServices = servicesByTrainer.Select(st => new TrainerServiceItemViewModel // ✅ İsim değişti
                {
                    ServicesByTrainerId = st.Id,
                    ServiceTypeId = st.ServiceTypeId,
                    ServiceName = st.serviceType?.Name ?? "Belirtilmemiş",
                    Description = st.serviceType?.Description ?? "Açıklama yok",
                    Duration = st.serviceType?.Duration ?? 0,
                    Price = st.serviceType?.Price ?? 0,
                    GymId = st.serviceType?.GymId ?? 0,
                    GymName = st.serviceType?.Gym?.Name ?? "Belirtilmemiş",
                    AppointmentCount = st.serviceType?.Appointments?.Count(a => a.TrainerId == trainer.Id) ?? 0
                }).ToList();

                // ============================================
                // 2. EKLENEBİLİR HİZMETLERİ AL (Dropdown için)
                // ============================================
                var allGymServices = await _unitOfWork.ServiceTypes.GetByGymIdAsync(trainer.GymId);
                var trainerServiceIds = servicesByTrainer.Select(ts => ts.ServiceTypeId).ToList();

                var availableServices = allGymServices
                    .Where(s => s.IsActive && !trainerServiceIds.Contains(s.Id)) // ✅ Henüz eklenmemiş olanlar
                    .Select(s => new AvailableServiceOption
                    {
                        ServiceTypeId = s.Id,
                        Name = s.Name,
                        Description = s.Description ?? "Açıklama yok",
                        Duration = s.Duration,
                        Price = s.Price
                    })
                    .ToList();

                // ============================================
                // 3. KOMBİNE VIEWMODEL OLUŞTUR
                // ============================================
                var viewModel = new TrainerServicesPageViewModel
                {
                    TrainerId = trainer.Id,
                    TrainerName = $"{user.FirstName} {user.LastName}",
                    GymName = trainer.Gym?.Name ?? "Belirtilmemiş",
                    CurrentServices = currentServices,
                    AvailableServices = availableServices
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Hata: " + ex.Message;
                return View(new TrainerServicesPageViewModel());
            }
        }

        // ============================================
        // HİZMET EKLE (POST) - Dropdown'dan
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddService(int serviceTypeId)
        {
            try
            {
                var userIdString = User.FindFirst("UserId")?.Value;

                var user = await _repository.GetByIdAsync(userIdString);
                if (user == null) return RedirectToAction("Index", "Home");

                var trainer = await _unitOfWork.Trainers.GetByUserIdAsync(user.Id);
                if (trainer == null)
                {
                    TempData["ErrorMessage"] = "Antrenör bilgileri bulunamadı.";
                    return RedirectToAction(nameof(ShowServices));
                }

                // ============================================
                // KONTROLLER
                // ============================================
                
                // 1. Servis zaten ekli mi?
                var exists = await _unitOfWork.ServicesByTrainers.TrainerHasServiceAsync(trainer.Id, serviceTypeId);
                if (exists)
                {
                    TempData["ErrorMessage"] = "❌ Bu hizmet zaten ekli!";
                    return RedirectToAction(nameof(ShowServices));
                }

                // 2. ServiceType var mı ve trainer'ın gym'ine ait mi?
                var serviceType = await _unitOfWork.ServiceTypes.GetByIdAsync(serviceTypeId);
                if (serviceType == null)
                {
                    TempData["ErrorMessage"] = "❌ Hizmet bulunamadı!";
                    return RedirectToAction(nameof(ShowServices));
                }

                if (serviceType.GymId != trainer.GymId)
                {
                    TempData["ErrorMessage"] = "❌ Bu hizmet sizin spor salonunuza ait değil!";
                    return RedirectToAction(nameof(ShowServices));
                }

                // ============================================
                // HİZMETİ EKLE
                // ============================================
                var serviceByTrainer = new ServicesByTrainer
                {
                    TrainerId = trainer.Id,
                    ServiceTypeId = serviceTypeId
                };

                await _unitOfWork.ServicesByTrainers.AddAsync(serviceByTrainer);
                await _unitOfWork.SaveChangesAsync();

                TempData["SuccessMessage"] = $"✅ {serviceType.Name} hizmeti başarıyla eklendi!";
                return RedirectToAction(nameof(ShowServices));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"❌ Hata: {ex.Message}";
                return RedirectToAction(nameof(ShowServices));
            }
        }

        // ============================================
        // HİZMET SİL (POST)
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteService(int id)
        {
            try
            {
                var userIdString = User.FindFirst("UserId")?.Value;

                var user = await _repository.GetByIdAsync(userIdString);
                if (user == null) return RedirectToAction("Login", "Account");

                var trainer = await _unitOfWork.Trainers.GetByUserIdAsync(user.Id);
                if (trainer == null)
                {
                    TempData["ErrorMessage"] = "Antrenör bilgileri bulunamadı.";
                    return RedirectToAction(nameof(ShowServices));
                }

                // ServicesByTrainer kaydını al
                var serviceByTrainer = await _unitOfWork.ServicesByTrainers.GetByIdAsync(id);
                
                if (serviceByTrainer == null || serviceByTrainer.TrainerId != trainer.Id)
                {
                    TempData["ErrorMessage"] = "❌ Yetkiniz yok veya hizmet bulunamadı.";
                    return RedirectToAction(nameof(ShowServices));
                }

                // Bu servisten aktif randevu var mı kontrol et
                var appointments = await _unitOfWork.Appointments.GetByTrainerIdAsync(trainer.Id);
                var hasActiveAppointments = appointments.Any(a => 
                    a.ServiceTypeId == serviceByTrainer.ServiceTypeId &&
                    a.AppointmentDate >= DateTime.Today &&
                    (a.Status == "Pending" || a.Status == "Confirmed"));

                if (hasActiveAppointments)
                {
                    TempData["ErrorMessage"] = "❌ Bu hizmetten aktif randevularınız var! Önce randevuları iptal edin.";
                    return RedirectToAction(nameof(ShowServices));
                }

                // Hizmeti sil
                await _unitOfWork.ServicesByTrainers.DeleteAsync(serviceByTrainer);
                await _unitOfWork.SaveChangesAsync();

                TempData["SuccessMessage"] = "✅ Hizmet başarıyla kaldırıldı!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"❌ Hata: {ex.Message}";
            }

            return RedirectToAction(nameof(ShowServices));
        }
    }
}
