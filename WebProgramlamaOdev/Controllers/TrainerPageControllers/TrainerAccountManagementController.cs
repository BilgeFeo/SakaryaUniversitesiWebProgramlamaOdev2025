using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProgramlamaOdev.DTOs;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;

namespace WebProgramlamaOdev.Controllers
{
    [Authorize(Roles = "Trainer")]
    public class TrainerAccountManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<ApplicationUser> _repository;


        public TrainerAccountManagementController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IRepository<ApplicationUser> repository)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _repository = repository;
        }

        
        public async Task<IActionResult> UpdateProfile()
        {
            try
            {
              
                var userIdString = User.FindFirst("UserId")?.Value;
                
                var user = await _repository.GetByIdAsync(userIdString);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                
                var trainer = await _unitOfWork.Trainers.GetByUserIdWithDetailsAsync(user.Id);
                if (trainer == null)
                {
                    TempData["ErrorMessage"] = "Antrenör bilgileri bulunamadı.";
                    return RedirectToAction("Index", "TrainerHome");
                }

                ViewBag.TrainerName = $"{user.FirstName} {user.LastName}";
                ViewBag.GymName = trainer.Gym?.Name ?? "Belirtilmemiş";

                
                var dto = new UpdateTrainerProfileDto
                {
                    Id = trainer.Id,
                    UserId = trainer.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    Specialization = trainer.Specialization ?? string.Empty,
                    IsActive = trainer.IsActive,
                    GymId = trainer.GymId,
                    GymName = trainer.Gym?.Name ?? "Belirtilmemiş"
                };

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return RedirectToAction("Index", "TrainerHome");
            }
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateTrainerProfileDto model)
        {
            if (!ModelState.IsValid)
            {
                
                var tempTrainer = await _unitOfWork.Trainers.GetByUserIdWithDetailsAsync(model.UserId);
                if (tempTrainer != null)
                {
                    ViewBag.TrainerName = $"{model.FirstName} {model.LastName}";
                    ViewBag.GymName = tempTrainer.Gym?.Name ?? "Belirtilmemiş";
                    model.GymName = tempTrainer.Gym?.Name ?? "Belirtilmemiş"; 
                }
                return View(model);
            }

            try
            {
                var userIdString = User.FindFirst("UserId")?.Value;

                var user = await _repository.GetByIdAsync(userIdString);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Oturum bulunamadı. Lütfen tekrar giriş yapın.";
                    return RedirectToAction("Index", "Home");
                }

                
                var trainer = await _unitOfWork.Trainers.GetByUserIdWithDetailsAsync(user.Id);
                if (trainer == null)
                {
                    TempData["ErrorMessage"] = "Antrenör kaydı bulunamadı.";
                    return RedirectToAction("Index", "TrainerHome");
                }

                
                if (trainer.UserId != user.Id)
                {
                    TempData["ErrorMessage"] = "Yetkiniz yok! Sadece kendi bilgilerinizi güncelleyebilirsiniz.";
                    return RedirectToAction("Index", "TrainerHome");
                }

                
                user.FirstName = model.FirstName.Trim();
                user.LastName = model.LastName.Trim();
                user.Email = model.Email.Trim();
                user.UserName = model.Email.Trim(); 
                user.PhoneNumber = model.PhoneNumber.Trim();

                var updateUserResult = await _userManager.UpdateAsync(user);

                if (!updateUserResult.Succeeded)
                {
                    foreach (var error in updateUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    
                    ViewBag.TrainerName = $"{model.FirstName} {model.LastName}";
                    ViewBag.GymName = trainer.Gym?.Name ?? "Belirtilmemiş";
                    model.GymName = trainer.Gym?.Name ?? "Belirtilmemiş"; 

                    return View(model);
                }

                
                trainer.Specialization = model.Specialization?.Trim() ?? string.Empty;
                trainer.IsActive = model.IsActive;

                

                await _unitOfWork.Trainers.UpdateAsync(trainer);
                await _unitOfWork.SaveChangesAsync();

                TempData["SuccessMessage"] = "✅ Bilgileriniz başarıyla güncellendi!";
                return RedirectToAction(nameof(UpdateProfile));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "❌ Güncelleme hatası: " + ex.Message;

                
                var errorTrainer = await _unitOfWork.Trainers.GetByUserIdWithDetailsAsync(model.UserId);
                if (errorTrainer != null)
                {
                    ViewBag.TrainerName = $"{model.FirstName} {model.LastName}";
                    ViewBag.GymName = errorTrainer.Gym?.Name ?? "Belirtilmemiş";
                    model.GymName = errorTrainer.Gym?.Name ?? "Belirtilmemiş";
                }

                return View(model);
            }
        }
    }
}
