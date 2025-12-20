using System.Text.Json.Serialization;

namespace WebProgramlamaOdev.ViewModels
{
    // Sayfadaki Form İçin Model
    public class AIPlanRequestViewModel
    {
        public int Age { get; set; }
        public int Height { get; set; } // cm
        public decimal Weight { get; set; } // kg
        public string Gender { get; set; } = "Erkek";
        public string Goal { get; set; } = "Kilo Vermek"; // Kilo Alma, Kas Yapma vb.
        public string ActivityLevel { get; set; } = "Orta"; // Düşük, Yüksek

        // Sonuçları Göstermek İçin
        public GeminiPlanResponse? Result { get; set; }
    }

    // Gemini'den Dönecek JSON Cevabı İçin Model
    public class GeminiPlanResponse
    {
        [JsonPropertyName("diet_plan")]
        public string DietPlan { get; set; } // HTML formatında liste

        [JsonPropertyName("workout_plan")]
        public string WorkoutPlan { get; set; } // HTML formatında liste

        [JsonPropertyName("recommendations")]
        public string Recommendations { get; set; } // Genel tavsiyeler
    }
}