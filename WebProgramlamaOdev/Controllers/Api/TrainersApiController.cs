using Microsoft.AspNetCore.Mvc;
using WebProgramlamaOdev.DTOs;
using WebProgramlamaOdev.Repositories.Interfaces;

namespace WebProgramlamaOdev.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainersApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainersApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrainers()
        {
            // Tüm aktif antrenörleri, User ve Gym bilgileriyle birlikte çekiyoruz
            var trainers = await _unitOfWork.Trainers.GetActiveTrainersAsync();

            var trainerDtos = trainers.Select(t => new TrainerApiDto
            {
                Id = t.Id,
                // İsim Soyisim Birleştirme
                FullName = $"{t.User.FirstName} {t.User.LastName}",
                Specialization = t.Specialization ?? "Genel Fitness",
                GymName = t.Gym?.Name ?? "Bağımsız",
                // İletişim Bilgileri
                Email = t.User.Email ?? "Belirtilmemiş",
                PhoneNumber = t.User.PhoneNumber ?? "Belirtilmemiş",
                // İstatistik (Opsiyonel)
                ActiveClientCount = t.Appointments?.Count(a => a.Status == "Confirmed") ?? 0
            });

            return Ok(trainerDtos);
        }
    }
}