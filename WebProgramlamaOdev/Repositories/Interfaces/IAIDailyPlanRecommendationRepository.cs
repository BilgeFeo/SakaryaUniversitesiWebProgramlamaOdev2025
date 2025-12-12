using WebProgramlamaOdev.Models;

namespace WebProgramlamaOdev.Repositories.Interfaces
{
    public interface IAIDailyPlanRecommendationRepository : IRepository<AIDailyPlanRecommendation>
    {
        Task<IEnumerable<AIDailyPlanRecommendation>> GetByMemberIdAsync(int memberId);
        Task<IEnumerable<AIDailyPlanRecommendation>> GetByRequestTypeAsync(string requestType);
        Task<IEnumerable<AIDailyPlanRecommendation>> GetMemberRecommendationsByTypeAsync(int memberId, string requestType);
        Task<AIDailyPlanRecommendation?> GetLatestByMemberAsync(int memberId);
        Task<AIDailyPlanRecommendation?> GetLatestByMemberAndTypeAsync(int memberId, string requestType);
        Task<IEnumerable<AIDailyPlanRecommendation>> GetRecentRecommendationsAsync(int memberId, int count);
    }
}
