using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProgramlamaOdev.Data;
using WebProgramlamaOdev.DTOs;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;
using WebProgramlamaOdev.Services.AdminPageServices;
using WebProgramlamaOdev.ViewModels;

namespace WebProgramlamaOdev.Controllers.AdminPageControllers

{

    public class AdminHomeController : Controller
    {
        
        private readonly IGymManagementService _createGymService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminHomeController(
            IGymManagementService createGymService,
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)  
        {
            _createGymService = createGymService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;  
        }


        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> ShowGyms()
        {
            try
            {
                // 1. Veritabanından tüm gym'leri çek
                var gyms = await _unitOfWork.Gyms.GetAllAsync();

                // 2. ViewModel'e dönüştür
                var gymViewModels = gyms.Select(g => new GymListViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    Email = g.User?.Email ?? "Belirtilmemiş",
                    PhoneNumber = g.User?.PhoneNumber ?? "Belirtilmemiş",
                    Address = g.Address,
                    OpeningTime = g.OpeningTime.ToString(@"hh\:mm"),
                    ClosingTime = g.ClosingTime.ToString(@"hh\:mm"),
                    IsActive = g.IsActive,
                    ServiceCount = g.ServiceList?.Count ?? 0,
                    TrainerCount = g.TrainerList?.Count ?? 0
                }).ToList();

                return View(gymViewModels);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Spor salonları yüklenirken bir hata oluştu: " + ex.Message;
                return View(new List<GymListViewModel>());
            }
        }

        [HttpGet]
        public IActionResult CreateGym()
        {
            return View();
        }





        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGymRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateGym", model); 
            }

            try
            {
                var result = await _createGymService.CreateGymAndSaveOnDatabase(model);

                if (result)
                {
                    TempData["SuccessMessage"] = $"{model.Name} başarıyla eklendi!";
                    return RedirectToAction(nameof(ShowGyms));
                }
                else
                {
                    TempData["ErrorMessage"] = "Salon eklenirken bir hata oluştu.";
                    return View("CreateGym", model);  
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Hata: " + ex.Message;
                return View("CreateGym", model);  
            }
        }




        [HttpGet]
        public async Task<IActionResult> EditGym(int id)
        {
            try
            {
                var gym = await _unitOfWork.Gyms.GetByIdAsync(id);

                if (gym == null)
                {
                    TempData["ErrorMessage"] = "Düzenlenecek spor salonu bulunamadı.";
                    return RedirectToAction(nameof(ShowGyms));
                }

                var dto = new UpdateGymRequestDto
                {
                    Id = gym.Id,
                    Name = gym.Name,
                    Email = gym.User?.Email ?? string.Empty,
                    PhoneNumber = gym.User?.PhoneNumber ?? string.Empty,
                    Address = gym.Address,
                    OpeningTime = gym.OpeningTime,
                    ClosingTime = gym.ClosingTime,
                    IsActive = gym.IsActive,
                    UserId = gym.UserId
                };

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Gym bilgileri yüklenirken hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(ShowGyms));
            }
        }

        
        [HttpPost] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGym(UpdateGymRequestDto model) 
        {
            if (!ModelState.IsValid)
            {
                return View("EditGym", model);  
            }

            try
            {
                var gym = await _unitOfWork.Gyms.GetByIdAsync(model.Id);

                if (gym == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek spor salonu bulunamadı.";
                    return RedirectToAction(nameof(ShowGyms));
                }

                

                
                gym.Name = model.Name;
                gym.Address = model.Address;
                gym.OpeningTime = model.OpeningTime;
                gym.ClosingTime = model.ClosingTime;
                gym.IsActive = model.IsActive;

                
                if (gym.User != null)
                {
                    var emailChanged = gym.User.Email != model.Email;
                    var phoneChanged = gym.User.PhoneNumber != model.PhoneNumber;

                    if (emailChanged || phoneChanged)
                    {
                        gym.User.Email = model.Email;
                        gym.User.UserName = model.Email;
                        gym.User.PhoneNumber = model.PhoneNumber;

                        var updateResult = await _userManager.UpdateAsync(gym.User);

                        if (!updateResult.Succeeded)
                        {
                            foreach (var error in updateResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }

                            return View("EditGym", model);  
                        }
                    }
                }

               
                await _unitOfWork.Gyms.UpdateAsync(gym);
                await _unitOfWork.SaveChangesAsync();  

                TempData["SuccessMessage"] = $"{gym.Name} başarıyla güncellendi.";
                return RedirectToAction(nameof(ShowGyms));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Güncelleme sırasında hata oluştu: " + ex.Message;
                return View("EditGym", model); 
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGym(int id)
        {
            try
            {
                var gym = await _unitOfWork.Gyms.GetWithServicesAndTrainersAsync(id);

                if (gym == null)
                {
                    TempData["ErrorMessage"] = "Silinmek istenen spor salonu bulunamadı.";
                    return RedirectToAction(nameof(ShowGyms));
                }

                if (gym.TrainerList.Any())
                {
                    TempData["ErrorMessage"] = $"Bu salon silinemez! {gym.TrainerList.Count} antrenör var.";
                    return RedirectToAction(nameof(ShowGyms));
                }

                if (gym.ServiceList.Any())
                {
                    TempData["ErrorMessage"] = $"Bu salon silinemez! {gym.ServiceList.Count} servis var.";
                    return RedirectToAction(nameof(ShowGyms));
                }

                string gymName = gym.Name;
                string userId = gym.UserId;

                await _unitOfWork.Gyms.DeleteAsync(gym);
                await _unitOfWork.SaveChangesAsync();

                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var deleteUserResult = await _userManager.DeleteAsync(user);

                    if (!deleteUserResult.Succeeded)
                    {
                        TempData["ErrorMessage"] = $"{gymName} silindi ama kullanıcı hesabı silinemedi: "
                            + string.Join(", ", deleteUserResult.Errors.Select(e => e.Description));
                        return RedirectToAction(nameof(ShowGyms));
                    }
                }

                TempData["SuccessMessage"] = $"{gymName} ve ilgili kullanıcı hesabı başarıyla silindi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Silme hatası: " + ex.Message;
            }

            return RedirectToAction(nameof(ShowGyms));
        }




        public IActionResult ShowServices() 
        {
            return View();
        }

        public IActionResult ShowTrainers()
        {
            return View();
        }

        public IActionResult ShowAppointments()
        {
            return View();
        }



        public IActionResult ShowMembers()
        {
            return View();
        }
    
    }
}
