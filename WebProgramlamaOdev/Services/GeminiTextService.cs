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
            // AppSettings.json'dan anahtarı okuyoruz
            _apiKey = configuration["GeminiSettings:ApiKey"];
            _httpClient = new HttpClient();
        }

        public async Task<GeminiPlanResponse> GeneratePlanAsync(AIPlanRequestViewModel request)
        {
            // Google Gemini API Adresi
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";

            // Yapay Zekaya Gönderilecek Emir (Prompt)
            string prompt = $@"
                Sen profesyonel bir fitness koçu ve diyetisyensin.
                Aşağıdaki özelliklere sahip kullanıcı için kişiselleştirilmiş bir plan oluştur:
                
                Kullanıcı Bilgileri:
                - Yaş: {request.Age}
                - Boy: {request.Height} cm
                - Kilo: {request.Weight} kg
                - Cinsiyet: {request.Gender}
                - Hedef: {request.Goal}
                - Aktivite Seviyesi: {request.ActivityLevel}

                Lütfen cevabını SADECE geçerli bir JSON formatında ver. Başka hiçbir metin veya markdown (```json gibi) kullanma.
                
                JSON Şeması şu şekilde olmalı:
                {{
                    ""diet_plan"": ""Günlük örnek diyet listesi (HTML <ul> ve <li> etiketleri kullanarak, Türkçe)"",
                    ""workout_plan"": ""Haftalık egzersiz programı (HTML <ul> ve <li> etiketleri kullanarak, Türkçe)"",
                    ""recommendations"": ""3-4 maddelik önemli tavsiyeler (HTML <ol> ve <li> etiketleri kullanarak, Türkçe)""
                }}
            ";

            // JSON Body Hazırlığı
            var requestBody = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            // İsteği Gönder
            var response = await _httpClient.PostAsync(url, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API Hatası ({response.StatusCode}): {errorMsg}");
            }

            var responseString = await response.Content.ReadAsStringAsync();

            try
            {
                // Gelen karmaşık JSON cevabını parçala
                using var doc = JsonDocument.Parse(responseString);
                var textResult = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                // Temizlik: Bazen AI ```json ... ``` etiketiyle döner, onları temizleyelim
                textResult = textResult.Replace("```json", "").Replace("```", "").Trim();

                // String'i nesneye çevir
                var plan = JsonSerializer.Deserialize<GeminiPlanResponse>(textResult);
                return plan;
            }
            catch (Exception ex)
            {
                // AI bazen bozuk JSON dönebilir, loglamak için
                throw new Exception("AI cevabı işlenirken hata oluştu: " + ex.Message);
            }
        }
    }
}