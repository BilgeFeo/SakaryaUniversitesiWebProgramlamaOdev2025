using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebProgramlamaOdev.ViewModels;

namespace WebProgramlamaOdev.Services
{
    public class GeminiTextService : IGeminiTextService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public GeminiTextService(IConfiguration configuration)
        {
            _apiKey = configuration["GeminiSettings:ApiKey"];
            _httpClient = new HttpClient();
        }

        public async Task<GeminiPlanResponse> GeneratePlanAsync(AIPlanRequestViewModel request)
        {
            // ✅ DÜZELTME: Model ismini "gemini-1.5-flash-latest" yaptık.
            // Eğer bu da çalışmazsa "gemini-pro" yazarak deneyebilirsin.
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";

            string prompt = $@"
                Sen profesyonel bir fitness koçu ve diyetisyensin.
                Aşağıdaki özelliklere sahip kullanıcı için Türkçe bir plan hazırla:
                - Yaş: {request.Age}
                - Boy: {request.Height} cm
                - Kilo: {request.Weight} kg
                - Cinsiyet: {request.Gender}
                - Hedef: {request.Goal}
                - Aktivite Seviyesi: {request.ActivityLevel}

                Lütfen cevabı SADECE geçerli bir JSON formatında ver. Başka metin veya markdown (```json) kullanma.
                JSON Şeması:
                {{
                    ""diet_plan"": ""Günlük örnek diyet listesi (HTML <ul> ve <li> etiketleri ile)"",
                    ""workout_plan"": ""Haftalık egzersiz programı (HTML <ul> ve <li> etiketleri ile)"",
                    ""recommendations"": ""3-4 maddelik tavsiyeler (HTML <ol> ve <li> etiketleri ile)""
                }}
            ";

            var requestBody = new
            {
                contents = new[] { new { parts = new[] { new { text = prompt } } } }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                // Hata mesajını daha okunaklı fırlatalım
                throw new Exception($"Gemini API Hatası ({response.StatusCode}): {errorMsg}");
            }

            var responseString = await response.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(responseString);

                // Cevap yapısı bazen değişebilir, güvenli erişim sağlayalım
                var candidates = doc.RootElement.GetProperty("candidates");
                if (candidates.GetArrayLength() == 0)
                    throw new Exception("AI boş cevap döndürdü.");

                var textResult = candidates[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                // Temizlik
                if (!string.IsNullOrEmpty(textResult))
                {
                    textResult = textResult.Replace("```json", "").Replace("```", "").Trim();
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<GeminiPlanResponse>(textResult, options);
            }
            catch (Exception ex)
            {
                throw new Exception($"Veri işlenirken hata: {ex.Message}. Gelen Ham Veri: {responseString}");
            }
        }
    }
}