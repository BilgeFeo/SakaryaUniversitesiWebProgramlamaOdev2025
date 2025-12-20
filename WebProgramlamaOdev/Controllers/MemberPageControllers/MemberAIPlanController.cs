using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;
using WebProgramlamaOdev.Services;
using WebProgramlamaOdev.ViewModels;

namespace WebProgramlamaOdev.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberAIPlanController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGeminiTextService _geminiService;

        public MemberAIPlanController(IUnitOfWork unitOfWork, IGeminiTextService geminiService)
        {
            _unitOfWork = unitOfWork;
            _geminiService = geminiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Kullanıcının mevcut bilgilerini çekip forma otomatik dolduralım
            var userId = User.FindFirst("UserId")?.Value;
            var member = await _unitOfWork.Members.GetByUserIdAsync(userId); //

            var model = new AIPlanRequestViewModel();

            if (member != null)
            {
                model.Height = member.Height ?? 170;
                model.Weight = member.Weight ?? 70;
                model.Age = member.Age ?? 25; // Member modelindeki Age property'sini kullanıyoruz
                model.Gender = member.Gender ?? "Erkek";
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Generate(AIPlanRequestViewModel model)
        {
            if (!ModelState.IsValid) return View("Index", model);

            try
            {
                var userId = User.FindFirst("UserId")?.Value;
                var member = await _unitOfWork.Members.GetByUserIdAsync(userId);

                // 1. Servisten Planı İste
                var planResult = await _geminiService.GeneratePlanAsync(model);

                // 2. Veritabanına Kaydet (Geçmişte ne önerildiğini tutmak için)
                // AIDailyPlanRecommendation tablosunu kullanıyoruz
                var recommendation = new AIDailyPlanRecommendation
                {
                    MemberId = member.Id,
                    RequestType = "FullPlan", // Bu tip metin tabanlı plan
                    CreatedDate = DateTime.Now,
                    InputData = JsonSerializer.Serialize(model), // Girilen boy/kilo vs.
                    AIResponse = JsonSerializer.Serialize(planResult), // Dönen plan
                    GeneratedImagePath = "/images/ai_plan_placeholder.jpg" // Resim yoksa varsayılan
                };

                await _unitOfWork.AIRecommendations.AddAsync(recommendation);
                await _unitOfWork.SaveChangesAsync();

                // 3. Sonucu Göster
                model.Result = planResult;
                return View("Index", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "AI Servis Hatası: " + ex.Message);
                return View("Index", model);
            }
        }
    }
}