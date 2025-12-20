using System.Net.Http.Headers;
using System.Text.Json;
using WebProgramlamaOdev.Services;

namespace WebProgramlamaOdev.Services
{
    public class HuggingFaceAIPhotoService : IAIPhotoService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly HttpClient _httpClient;

        // ✅ GÜNCELLENEN SATIR: Eski "api-inference" adresi yerine "router" adresi
        private const string ModelUrl = "https://router.huggingface.co/hf-inference/models/stabilityai/stable-diffusion-xl-base-1.0";

        public HuggingFaceAIPhotoService(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            _httpClient = new HttpClient();

            var token = _configuration["HuggingFaceSettings:ApiKey"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<string> TransformBodyAsync(string localImagePath, string intensity)
        {
            // 1. Prompt Hazırla
            string prompt = intensity switch
            {
                "Light" => "full body photo of a fitness model male, lean muscle, athletic physique, gym background, highly detailed, photorealistic, 8k, masterpiece",
                "Medium" => "full body photo of a muscular man, six pack abs, strong arms, gym workout lighting, highly detailed, photorealistic, 8k, masterpiece",
                "Heavy" => "full body photo of a massive bodybuilder, extreme muscles, vascular, professional bodybuilding stage, highly detailed, photorealistic, 8k, masterpiece",
                _ => "photo of a fit person working out"
            };

            // 2. İstek Gövdesini Hazırla
            var requestData = new
            {
                inputs = prompt,
                parameters = new
                {
                    negative_prompt = "cartoon, drawing, illustration, low quality, blur, deformed, ugly, extra limbs" // Sonuçların daha gerçekçi olması için negatif prompt
                }
            };

            // 3. API'ye Gönder
            var jsonContent = new StringContent(JsonSerializer.Serialize(requestData), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(ModelUrl, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                // HTML hatası dönerse sadece özetini gösterelim
                if (error.Contains("<!doctype html>")) error = "Sunucu adresi hatası veya model yükleniyor.";

                throw new Exception($"Hugging Face Hatası: {response.StatusCode} - {error}. Lütfen biraz bekleyip tekrar deneyin.");
            }

            // 4. Gelen Resim Byte'larını Oku
            var imageBytes = await response.Content.ReadAsByteArrayAsync();

            // 5. Resmi Kaydet
            string outputFileName = $"ai_gen_{Guid.NewGuid()}.jpg";
            string outputFolder = Path.Combine(_env.WebRootPath, "uploads", "ai_output");

            if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);

            string savePath = Path.Combine(outputFolder, outputFileName);
            await File.WriteAllBytesAsync(savePath, imageBytes);

            return $"/uploads/ai_output/{outputFileName}";
        }
    }
}