using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;
using WebProgramlamaOdev.ViewModels;

namespace WebProgramlamaOdev.Controllers
{
    [Authorize(Roles = "Trainer")]
    public class TrainerHomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<ApplicationUser> _repository;

        
        public TrainerHomeController(
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
            try
            {
                var userIdString = User.FindFirst("UserId")?.Value;
                var user = await _repository.GetByIdAsync(userIdString);


                if (user == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                // Trainer bilgilerini al
                var trainer = await _unitOfWork.Trainers.GetByUserIdWithDetailsAsync(user.Id);
                if (trainer == null)
                {
                    TempData["ErrorMessage"] = "Antrenör bilgileri bulunamadı.";
                    return RedirectToAction("Index", "Home");
                }

                // ViewBag'e trainer bilgilerini ekle (Layout için)
                ViewBag.TrainerName = $"{user.FirstName} {user.LastName}";
                ViewBag.GymName = trainer.Gym?.Name ?? "Belirtilmemiş";

                // İstatistikleri hesapla
                var allAppointments = await _unitOfWork.Appointments.GetByTrainerIdAsync(trainer.Id);
                var services = await _unitOfWork.ServicesByTrainers.GetByTrainerIdAsync(trainer.Id);

                var viewModel = new TrainerDashboardViewModel
                {
                    TrainerId = trainer.Id,
                    TrainerName = $"{user.FirstName} {user.LastName}",
                    GymName = trainer.Gym?.Name ?? "Belirtilmemiş",
                    Specialization = trainer.Specialization ?? "Belirtilmemiş",
                    IsActive = trainer.IsActive,
                    
                    TotalServices = services.Count(),
                    TotalAppointments = allAppointments.Count(),
                    PendingAppointments = allAppointments.Count(a => a.Status == "Pending"),
                    TodayAppointments = allAppointments.Count(a => 
                        a.AppointmentDate.Date == DateTime.Today &&
                        (a.Status == "Pending" || a.Status == "Confirmed")),
                    ConfirmedAppointments = allAppointments.Count(a => a.Status == "Confirmed")
                };

                // Yaklaşan randevuları al (bugünden itibaren 7 gün)
                var upcomingAppointments = allAppointments
                    .Where(a => a.AppointmentDate >= DateTime.Today && 
                               (a.Status == "Pending" || a.Status == "Confirmed"))
                    .OrderBy(a => a.AppointmentDate)
                    .ThenBy(a => a.StartTime)
                    .Take(5)
                    .Select(a => new UpcomingAppointmentViewModel
                    {
                        Id = a.Id,
                        MemberName = $"{a.Member?.User?.FirstName} {a.Member?.User?.LastName}",
                        ServiceName = a.ServiceType?.Name ?? "Belirtilmemiş",
                        AppointmentDate = a.AppointmentDate,
                        StartTime = a.StartTime,
                        EndTime = a.EndTime,
                        Status = a.Status,
                        Price = a.TotalPrice
                    })
                    .ToList();

                viewModel.UpcomingAppointments = upcomingAppointments;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return View(new TrainerDashboardViewModel());
            }
        }
    }
}
