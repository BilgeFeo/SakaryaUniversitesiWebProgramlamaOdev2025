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
    }
}