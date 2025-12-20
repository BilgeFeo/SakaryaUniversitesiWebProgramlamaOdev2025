using Microsoft.AspNetCore.Http;
using WebProgramlamaOdev.Models;
using System.Collections.Generic;

namespace WebProgramlamaOdev.ViewModels
{
    public class AIBodyAnalysisViewModel
    {
        // Geçmiş Analizler Listesi
        public List<AIDailyPlanRecommendation> PreviousAnalyses { get; set; } = new();

        // Form Verileri
        public IFormFile? UploadedImage { get; set; }
        public string SelectedIntensity { get; set; } = "Medium"; // Light, Medium, Heavy

        // Sonuç Ekranı İçin (Post işleminden sonra dolacak)
        public string? ResultOriginalImage { get; set; }
        public string? ResultGeneratedImage { get; set; }
        public bool IsProcessed { get; set; } = false;
    }
}