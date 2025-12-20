using WebProgramlamaOdev.ViewModels;

namespace WebProgramlamaOdev.Services
{
    public interface IGeminiTextService
    {
        Task<GeminiPlanResponse> GeneratePlanAsync(AIPlanRequestViewModel request);
    }
}
