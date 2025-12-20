using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebProgramlamaOdev.DTOs;
using WebProgramlamaOdev.Repositories.Interfaces;
using WebProgramlamaOdev.ViewModels;

namespace WebProgramlamaOdev.Controllers.AdminPageControllers
{
    [Authorize(Roles = "Gym")]
    public class AdminServiceManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminServiceManagementController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
        public async Task<IActionResult> ShowServices()
        {
            try
            {
                var services = await _unitOfWork.ServiceTypes.GetAllAsync();

                var serviceViewModels = services.Select(s => new ServiceListViewModel
                {
                    Id = s.Id,
                    GymId = s.GymId,
                    GymName = s.Gym?.Name ?? "Belirtilmemiş",
                    Name = s.Name,
                    Description = s.Description,
                    Duration = s.Duration,
                    Price = s.Price,
                    IsActive = s.IsActive,
                    TrainerCount = s.ServicesByTrainers?.Count ?? 0,
                    AppointmentCount = s.Appointments?.Count ?? 0
                }).ToList();

                return View(serviceViewModels);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Servisler yüklenirken bir hata oluştu: " + ex.Message;
                return View(new List<ServiceListViewModel>());
            }
        }

        // ============================================
        // CREATE SERVICE - Yeni Servis Ekleme (GET)
        // ============================================
        public async Task<IActionResult> CreateService()
        {
            try
            {
                // Gym dropdown için gym listesi
                var gyms = await _unitOfWork.Gyms.GetAllAsync();
                ViewBag.Gyms = new SelectList(gyms.Where(g => g.IsActive), "Id", "Name");

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Form yüklenirken hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(ShowServices));
            }
        }

        // ============================================
        // CREATE SERVICE - Yeni Servis Ekleme (POST)
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateService(CreateServiceRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                // Hata durumunda gym listesini tekrar yükle
                var gyms = await _unitOfWork.Gyms.GetAllAsync();
                ViewBag.Gyms = new SelectList(gyms.Where(g => g.IsActive), "Id", "Name");
                return View("CreateService", model);
            }

            try
            {
                var service = new ServiceType
                {
                    GymId = model.GymId,
                    Name = model.Name,
                    Description = model.Description,
                    Duration = model.Duration,
                    Price = model.Price,
                    IsActive = model.IsActive
                };

                await _unitOfWork.ServiceTypes.AddAsync(service);
                await _unitOfWork.SaveChangesAsync();

                TempData["SuccessMessage"] = $"{model.Name} başarıyla eklendi!";
                return RedirectToAction(nameof(ShowServices));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Servis eklenirken hata oluştu: " + ex.Message;

                // Hata durumunda gym listesini tekrar yükle
                var gyms = await _unitOfWork.Gyms.GetAllAsync();
                ViewBag.Gyms = new SelectList(gyms.Where(g => g.IsActive), "Id", "Name");
                return View("CreateService", model);
            }
        }

        // ============================================
        // EDIT SERVICE - Servis Düzenleme (GET)
        // ============================================
        [HttpGet]
        public async Task<IActionResult> EditService(int id)
        {
            try
            {
                var service = await _unitOfWork.ServiceTypes.GetByIdAsync(id);

                if (service == null)
                {
                    TempData["ErrorMessage"] = "Düzenlenecek servis bulunamadı.";
                    return RedirectToAction(nameof(ShowServices));
                }

                var dto = new UpdateServiceRequestDto
                {
                    Id = service.Id,
                    GymId = service.GymId,
                    Name = service.Name,
                    Description = service.Description,
                    Duration = service.Duration,
                    Price = service.Price,
                    IsActive = service.IsActive
                };

                // Gym dropdown için gym listesi
                var gyms = await _unitOfWork.Gyms.GetAllAsync();
                ViewBag.Gyms = new SelectList(gyms.Where(g => g.IsActive), "Id", "Name", service.GymId);

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Servis bilgileri yüklenirken hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(ShowServices));
            }
        }

        // ============================================
        // EDIT SERVICE - Servis Düzenleme (POST)
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditService(UpdateServiceRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                // Hata durumunda gym listesini tekrar yükle
                var gyms = await _unitOfWork.Gyms.GetAllAsync();
                ViewBag.Gyms = new SelectList(gyms.Where(g => g.IsActive), "Id", "Name", model.GymId);
                return View("EditService", model);
            }

            try
            {
                var service = await _unitOfWork.ServiceTypes.GetByIdAsync(model.Id);

                if (service == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek servis bulunamadı.";
                    return RedirectToAction(nameof(ShowServices));
                }

                // Servis bilgilerini güncelle
                service.GymId = model.GymId;
                service.Name = model.Name;
                service.Description = model.Description;
                service.Duration = model.Duration;
                service.Price = model.Price;
                service.IsActive = model.IsActive;

                await _unitOfWork.ServiceTypes.UpdateAsync(service);
                await _unitOfWork.SaveChangesAsync();

                TempData["SuccessMessage"] = $"{service.Name} başarıyla güncellendi.";
                return RedirectToAction(nameof(ShowServices));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Güncelleme sırasında hata oluştu: " + ex.Message;

                // Hata durumunda gym listesini tekrar yükle
                var gyms = await _unitOfWork.Gyms.GetAllAsync();
                ViewBag.Gyms = new SelectList(gyms.Where(g => g.IsActive), "Id", "Name", model.GymId);
                return View("EditService", model);
            }
        }

        // ============================================
        // DELETE SERVICE - Servis Silme (POST)
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteService(int id)
        {
            try
            {
                var service = await _unitOfWork.ServiceTypes.GetByIdAsync(id);

                if (service == null)
                {
                    TempData["ErrorMessage"] = "Silinmek istenen servis bulunamadı.";
                    return RedirectToAction(nameof(ShowServices));
                }

                // İlişkili veri kontrolü
                if (service.ServicesByTrainers != null && service.ServicesByTrainers.Any())
                {
                    TempData["ErrorMessage"] = $"Bu servis silinemez! {service.ServicesByTrainers.Count} antrenör bu servisi veriyor.";
                    return RedirectToAction(nameof(ShowServices));
                }

                if (service.Appointments != null && service.Appointments.Any())
                {
                    TempData["ErrorMessage"] = $"Bu servis silinemez! {service.Appointments.Count} randevu var.";
                    return RedirectToAction(nameof(ShowServices));
                }

                string serviceName = service.Name;

                // Servisi sil
                await _unitOfWork.ServiceTypes.DeleteAsync(service);
                await _unitOfWork.SaveChangesAsync();

                TempData["SuccessMessage"] = $"{serviceName} başarıyla silindi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Silme işlemi sırasında bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction(nameof(ShowServices));
        }
    }
}
