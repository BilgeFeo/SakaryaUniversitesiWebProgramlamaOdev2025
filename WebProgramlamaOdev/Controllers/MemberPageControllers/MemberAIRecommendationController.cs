using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;
using WebProgramlamaOdev.Services;
using WebProgramlamaOdev.ViewModels;

namespace WebProgramlamaOdev.Controllers
{
    [Authorize(Roles = "Member")] // Sadece üyeler erişsin
    public class MemberAIRecommendationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAIPhotoService _aiPhotoService; // Servisi inject ediyoruz

        public MemberAIRecommendationController(
            IUnitOfWork unitOfWork,
            IWebHostEnvironment webHostEnvironment,
            IAIPhotoService aiPhotoService)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _aiPhotoService = aiPhotoService;
        }

        // ============================================
        // INDEX: Geçmişi Listele ve Form Göster
        // ============================================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new AIBodyAnalysisViewModel();

            // 1. Üye Bilgisini Al
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            var member = await _unitOfWork.Members.GetByUserIdAsync(userId);
            if (member == null) return RedirectToAction("Index", "Home");

            // 2. Geçmiş "BodyPrediction" kayıtlarını çek
            var history = await _unitOfWork.AIRecommendations
                .GetMemberRecommendationsByTypeAsync(member.Id, "BodyPrediction");

            model.PreviousAnalyses = history.ToList();

            return View(model);
        }

        // ============================================
        // ANALYZE: Resmi İşle ve Kaydet
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Analyze(AIBodyAnalysisViewModel model)
        {
            // Tekrar üye bilgisini al
            var userId = User.FindFirst("UserId")?.Value;
            var member = await _unitOfWork.Members.GetByUserIdAsync(userId);
            if (member == null) return RedirectToAction("Login", "Account");

            if (model.UploadedImage != null && model.UploadedImage.Length > 0)
            {
                try
                {
                    // 1. Resmi Sunucuya Kaydet (wwwroot/uploads/ai_input)
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "ai_input");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.UploadedImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.UploadedImage.CopyToAsync(fileStream);
                    }

                    string webOriginalPath = "/uploads/ai_input/" + uniqueFileName;

                    // 2. AI Servisini Çağır
                    // "Light", "Medium", "Heavy" seçimine göre servis işlem yapacak
                    string generatedImageUrl = await _aiPhotoService.TransformBodyAsync(webOriginalPath, model.SelectedIntensity);

                    // 3. Veritabanına Kaydet (AIDailyPlanRecommendation Modelini Kullanarak)
                    var recommendation = new AIDailyPlanRecommendation
                    {
                        MemberId = member.Id,
                        RequestType = "BodyPrediction", // Tipi belirliyoruz
                        CreatedDate = DateTime.Now,
                        GeneratedImagePath = generatedImageUrl, // AI'dan gelen resim

                        // InputData içine orijinal resmi ve seçilen yoğunluğu JSON olarak gömüyoruz
                        InputData = JsonSerializer.Serialize(new
                        {
                            OriginalImage = webOriginalPath,
                            Intensity = model.SelectedIntensity
                        }),

                        AIResponse = "Success" // Veya API'den dönen ham JSON
                    };

                    await _unitOfWork.AIRecommendations.AddAsync(recommendation);
                    await _unitOfWork.SaveChangesAsync();

                    // 4. Sonuçları ViewModel'e Yükle
                    model.ResultOriginalImage = webOriginalPath;
                    model.ResultGeneratedImage = generatedImageUrl;
                    model.IsProcessed = true;
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "AI işlem hatası: " + ex.Message;
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Lütfen bir fotoğraf yükleyin.";
            }

            // Sayfayı tekrar yükle (Geçmiş listesini de güncelleyerek)
            var history = await _unitOfWork.AIRecommendations
                .GetMemberRecommendationsByTypeAsync(member.Id, "BodyPrediction");
            model.PreviousAnalyses = history.ToList();

            return View("Index", model);
        }
    }
}