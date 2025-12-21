using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;
using WebProgramlamaOdev.ViewModels;

namespace WebProgramlamaOdev.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberHomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<ApplicationUser> _repository;


        public MemberHomeController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IRepository<ApplicationUser> repository)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _repository = repository;
        }


        public async Task<IActionResult> Index()
        {
            
            var userIdString = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Index", "Home");
            }

            
            var member = await _unitOfWork.Members.GetByUserIdWithDetailsAsync(userIdString);

            if (member == null)
            {
                
                TempData["ErrorMessage"] = "Üye profil bilgileriniz bulunamadı.";
                return View(new MemberDashboardViewModel());
            }

            
            ViewBag.UserName = $"{member.User.FirstName} {member.User.LastName}";

            
            var activeAppointments = member.Appointments?.Count(a => a.Status == "Pending" || a.Status == "Confirmed") ?? 0;
            var aiPrograms = member.AIRecommendations?.Count ?? 0;

            
            var viewModel = new MemberDashboardViewModel
            {
                FullName = $"{member.User.FirstName} {member.User.LastName}",
                Email = member.User.Email ?? "-",
                PhoneNumber = member.User.PhoneNumber ?? "-",
                Gender = member.Gender == "Male" ? "Erkek" : (member.Gender == "Female" ? "Kadın" : member.Gender),
                DateOfBirth = member.DateOfBirth,
                Age = member.Age ?? 0, 
                Height = member.Height,
                Weight = member.Weight,
                ActiveAppointmentsCount = activeAppointments,
                AIProgramsCount = aiPrograms
            };

            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var userIdString = User.FindFirst("UserId")?.Value;
            var user = await _repository.GetByIdAsync(userIdString);


            if (user == null) return RedirectToAction("Index", "Home");

            // Repository üzerinden Member detaylarını çekiyoruz
            var member = await _unitOfWork.Members.GetByUserIdAsync(user.Id);

            var model = new MemberProfileUpdateViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,

                // Member tablosundan gelenler
                Height = member?.Height,
                Weight = member?.Weight,
                DateOfBirth = member?.DateOfBirth,
                Gender = member?.Gender ?? "Erkek"
            };

            return View(model);
        }

        // POST: Profil Güncelleme İşlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(MemberProfileUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userIdString = User.FindFirst("UserId")?.Value;
            var user = await _repository.GetByIdAsync(userIdString);


            if (user == null) return RedirectToAction("Index", "Home");

            // 1. Identity Bilgilerini Güncelle (Ad, Soyad, Email, Tel)
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email; // Genelde username email ile aynı tutulur
            user.PhoneNumber = model.PhoneNumber;

            var identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            // 2. Şifre Değiştirme Kontrolü
            if (!string.IsNullOrEmpty(model.CurrentPassword) && !string.IsNullOrEmpty(model.NewPassword))
            {
                var passwordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                    {
                        ModelState.AddModelError("", "Şifre değiştirilemedi: " + error.Description);
                    }
                    return View(model);
                }
            }

            // 3. Member (Fiziksel) Bilgilerini Güncelle
            var member = await _unitOfWork.Members.GetByUserIdAsync(user.Id);
            if (member != null)
            {
                member.Height = model.Height;
                member.Weight = model.Weight;
                member.DateOfBirth = model.DateOfBirth;
                member.Gender = model.Gender;

                await _unitOfWork.Members.UpdateAsync(member);
                await _unitOfWork.SaveChangesAsync(); // UnitOfWork ile kaydet
            }

            TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla güncellendi.";
            return RedirectToAction("Index"); // Ana sayfaya yönlendir
        }


    }
}