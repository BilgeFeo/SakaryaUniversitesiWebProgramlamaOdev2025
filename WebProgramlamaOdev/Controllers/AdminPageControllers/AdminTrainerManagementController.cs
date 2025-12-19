using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebProgramlamaOdev.Data;
using WebProgramlamaOdev.DTOs;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;
using WebProgramlamaOdev.ViewModels;

namespace WebProgramlamaOdev.Controllers.AdminPageControllers
{
    public class AdminTrainerManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public AdminTrainerManagementController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _context = context;     
        }

        // ============================================
        // SHOW TRAINERS - Antrenörleri Listele
        // ============================================
        public async Task<IActionResult> ShowTrainers()
        {
            try
            {
                var trainers = await _unitOfWork.Trainers.GetAllAsync();
                
                var trainerViewModels = trainers.Select(t => new TrainerListViewModel
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    GymId = t.GymId,
                    GymName = t.Gym?.Name ?? "Belirtilmemiş",
                    FirstName = t.User?.FirstName ?? string.Empty,
                    LastName = t.User?.LastName ?? string.Empty,
                    Email = t.User?.Email ?? "Belirtilmemiş",
                    PhoneNumber = t.User?.PhoneNumber ?? "Belirtilmemiş",
                    Specialization = t.Specialization ?? "Belirtilmemiş",
                    IsActive = t.IsActive,
                    ServiceCount = t.TrainerServiceTypes?.Count ?? 0,
                    AppointmentCount = t.Appointments?.Count ?? 0
                }).ToList();

                return View(trainerViewModels);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Antrenörler yüklenirken bir hata oluştu: " + ex.Message;
                return View(new List<TrainerListViewModel>());
            }
        }

        // ============================================
        // CREATE TRAINER - Yeni Antrenör Ekleme (GET)
        // ============================================
        public async Task<IActionResult> CreateTrainer()
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
                return RedirectToAction(nameof(ShowTrainers));
            }
        }

        // ============================================
        // CREATE TRAINER - Yeni Antrenör Ekleme (POST)
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainer(CreateTrainerRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                // Hata durumunda gym listesini tekrar yükle
                var gyms = await _unitOfWork.Gyms.GetAllAsync();
                ViewBag.Gyms = new SelectList(gyms.Where(g => g.IsActive), "Id", "Name");
                return View("CreateTrainer", model);
            }

            try
            {
                // 1. ApplicationUser oluştur
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserType = "Trainer",
                    EmailConfirmed = true
                };

                var createUserResult = await _userManager.CreateAsync(user, model.Password);

                if (!createUserResult.Succeeded)
                {
                    foreach (var error in createUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    // Hata durumunda gym listesini tekrar yükle
                    var gyms = await _unitOfWork.Gyms.GetAllAsync();
                    ViewBag.Gyms = new SelectList(gyms.Where(g => g.IsActive), "Id", "Name");
                    return View("CreateTrainer", model);
                }

                // 2. Trainer role ekle
               

                // 3. Trainer oluştur
                var trainer = new Trainer
                {
                    UserId = user.Id,
                    GymId = model.GymId,
                    Specialization = model.Specialization,
                    IsActive = model.IsActive
                };

                _context.Trainers.Add(trainer);
                var saveResult = await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"{model.FirstName} {model.LastName} başarıyla eklendi!";
                return RedirectToAction(nameof(ShowTrainers));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Antrenör eklenirken hata oluştu: " + ex.Message;
                
                // Hata durumunda gym listesini tekrar yükle
                var gyms = await _unitOfWork.Gyms.GetAllAsync();
                ViewBag.Gyms = new SelectList(gyms.Where(g => g.IsActive), "Id", "Name");
                return View("CreateTrainer", model);
            }
        }

        // ============================================
        // EDIT TRAINER - Antrenör Düzenleme (GET)
        // ============================================
        [HttpGet]
        public async Task<IActionResult> EditTrainer(int id)
        {
            try
            {
                // ✅ DÜZELTME: GetByIdWithDetailsAsync kullan (User ve Gym bilgilerini yükle)
                var trainer = await _unitOfWork.Trainers.GetByIdWithDetailsAsync(id);
                
                if (trainer == null)
                {
                    TempData["ErrorMessage"] = "Düzenlenecek antrenör bulunamadı.";
                    return RedirectToAction(nameof(ShowTrainers));
                }

                var dto = new UpdateTrainerRequestDto
                {
                    Id = trainer.Id,
                    UserId = trainer.UserId,
                    GymId = trainer.GymId,
                    FirstName = trainer.User?.FirstName ?? string.Empty,
                    LastName = trainer.User?.LastName ?? string.Empty,
                    Email = trainer.User?.Email ?? string.Empty,
                    PhoneNumber = trainer.User?.PhoneNumber ?? string.Empty,
                    Specialization = trainer.Specialization ?? string.Empty,
                    IsActive = trainer.IsActive
                };

                // Gym dropdown için gym listesi
                var gyms = await _unitOfWork.Gyms.GetAllAsync();
                ViewBag.Gyms = new SelectList(gyms.Where(g => g.IsActive), "Id", "Name", trainer.GymId);
                
                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Antrenör bilgileri yüklenirken hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(ShowTrainers));
            }
        }

        // ============================================
        // EDIT TRAINER - Antrenör Düzenleme (POST)
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrainer(UpdateTrainerRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                // Hata durumunda gym listesini tekrar yükle
                var gyms = await _unitOfWork.Gyms.GetAllAsync();
                ViewBag.Gyms = new SelectList(gyms.Where(g => g.IsActive), "Id", "Name", model.GymId);
                return View("EditTrainer", model);
            }

            try
            {
                // ✅ DÜZELTME: GetByIdWithDetailsAsync kullan
                var trainer = await _unitOfWork.Trainers.GetByIdWithDetailsAsync(model.Id);
                
                if (trainer == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek antrenör bulunamadı.";
                    return RedirectToAction(nameof(ShowTrainers));
                }

                // Trainer bilgilerini güncelle
                trainer.GymId = model.GymId;
                trainer.Specialization = model.Specialization;
                trainer.IsActive = model.IsActive;

                // ApplicationUser bilgilerini güncelle
                if (trainer.User != null)
                {
                    trainer.User.FirstName = model.FirstName;
                    trainer.User.LastName = model.LastName;
                    trainer.User.Email = model.Email;
                    trainer.User.UserName = model.Email;
                    trainer.User.PhoneNumber = model.PhoneNumber;

                    var updateResult = await _userManager.UpdateAsync(trainer.User);
                    
                    if (!updateResult.Succeeded)
                    {
                        foreach (var error in updateResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        
                        // Hata durumunda gym listesini tekrar yükle
                        var gyms = await _unitOfWork.Gyms.GetAllAsync();
                        ViewBag.Gyms = new SelectList(gyms.Where(g => g.IsActive), "Id", "Name", model.GymId);
                        return View("EditTrainer", model);
                    }
                }

                await _unitOfWork.Trainers.UpdateAsync(trainer);
                await _unitOfWork.SaveChangesAsync();
                
                TempData["SuccessMessage"] = $"{model.FirstName} {model.LastName} başarıyla güncellendi.";
                return RedirectToAction(nameof(ShowTrainers));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Güncelleme sırasında hata oluştu: " + ex.Message;
                
                // Hata durumunda gym listesini tekrar yükle
                var gyms = await _unitOfWork.Gyms.GetAllAsync();
                ViewBag.Gyms = new SelectList(gyms.Where(g => g.IsActive), "Id", "Name", model.GymId);
                return View("EditTrainer", model);
            }
        }

        // ============================================
        // DELETE TRAINER - Antrenör Silme (POST)
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            try
            {
                // 1. Trainer'ı detaylarıyla getir
                var trainer = await _unitOfWork.Trainers.GetByIdAsync(id);

                if (trainer == null)
                {
                    TempData["ErrorMessage"] = "❌ Silinmek istenen antrenör bulunamadı.";
                    return RedirectToAction(nameof(ShowTrainers));
                }

                string trainerName = $"{trainer.User?.FirstName} {trainer.User?.LastName}";
                string userId = trainer.UserId;

                
               
                var servicesByTrainerList = await _unitOfWork.ServicesByTrainers.GetByTrainerIdAsync(id);

                if (servicesByTrainerList != null && servicesByTrainerList.Any())
                {
                    int serviceCount = servicesByTrainerList.Count();

                    foreach (var serviceByTrainer in servicesByTrainerList)
                    {
                        await _unitOfWork.ServicesByTrainers.DeleteAsync(serviceByTrainer);
                    }

                    await _unitOfWork.SaveChangesAsync();

                    Console.WriteLine($"✅ {serviceCount} ServicesByTrainer kaydı silindi.");
                }

                
                var appointmentsList = await _unitOfWork.Appointments.GetByTrainerIdAsync(id);

                if (appointmentsList != null && appointmentsList.Any())
                {
                    int appointmentCount = appointmentsList.Count();

                   
                    foreach (var appointment in appointmentsList)
                    {
                        await _unitOfWork.Appointments.DeleteAsync(appointment);
                    }

                    
                    await _unitOfWork.SaveChangesAsync();

                   
                }

               
               

               
                await _unitOfWork.Trainers.DeleteAsync(trainer);
                await _unitOfWork.SaveChangesAsync();

               
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var deleteUserResult = await _userManager.DeleteAsync(user);

                    if (!deleteUserResult.Succeeded)
                    {
                        TempData["ErrorMessage"] = $"⚠️ {trainerName} silindi ama kullanıcı hesabı silinemedi: "
                            + string.Join(", ", deleteUserResult.Errors.Select(e => e.Description));
                        return RedirectToAction(nameof(ShowTrainers));
                    }
                }

                TempData["SuccessMessage"] = $"✅ {trainerName} ve tüm ilişkili kayıtlar başarıyla silindi!";
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                var innerMsg = dbEx.InnerException?.Message ?? dbEx.Message;
                TempData["ErrorMessage"] = $"❌ Database hatası: {innerMsg}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"❌ Silme hatası: {ex.Message}" +
                    (ex.InnerException != null ? $" | İç hata: {ex.InnerException.Message}" : "");
            }

            return RedirectToAction(nameof(ShowTrainers));
        }
    }
}
