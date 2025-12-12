using Microsoft.EntityFrameworkCore;
using WebProgramlamaOdev.Data;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;

namespace WebProgramlamaOdev.Repositories
{
    public class AIDailyPlanRecommendationRepository : Repository<AIDailyPlanRecommendation>, IAIDailyPlanRecommendationRepository
    {
        public AIDailyPlanRecommendationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AIDailyPlanRecommendation>> GetByMemberIdAsync(int memberId)
        {
            return await _dbSet
                .Include(ai => ai.Member)
                    .ThenInclude(m => m.User)
                .Where(ai => ai.MemberId == memberId)
                .OrderByDescending(ai => ai.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<AIDailyPlanRecommendation>> GetByRequestTypeAsync(string requestType)
        {
            return await _dbSet
                .Include(ai => ai.Member)
                    .ThenInclude(m => m.User)
                .Where(ai => ai.RequestType == requestType)
                .OrderByDescending(ai => ai.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<AIDailyPlanRecommendation>> GetMemberRecommendationsByTypeAsync(
            int memberId,
            string requestType)
        {
            return await _dbSet
                .Include(ai => ai.Member)
                    .ThenInclude(m => m.User)
                .Where(ai => ai.MemberId == memberId && ai.RequestType == requestType)
                .OrderByDescending(ai => ai.CreatedDate)
                .ToListAsync();
        }

        public async Task<AIDailyPlanRecommendation?> GetLatestByMemberAsync(int memberId)
        {
            return await _dbSet
                .Include(ai => ai.Member)
                    .ThenInclude(m => m.User)
                .Where(ai => ai.MemberId == memberId)
                .OrderByDescending(ai => ai.CreatedDate)
                .FirstOrDefaultAsync();
        }

        public async Task<AIDailyPlanRecommendation?> GetLatestByMemberAndTypeAsync(
            int memberId,
            string requestType)
        {
            return await _dbSet
                .Include(ai => ai.Member)
                    .ThenInclude(m => m.User)
                .Where(ai => ai.MemberId == memberId && ai.RequestType == requestType)
                .OrderByDescending(ai => ai.CreatedDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AIDailyPlanRecommendation>> GetRecentRecommendationsAsync(
            int memberId,
            int count)
        {
            return await _dbSet
                .Include(ai => ai.Member)
                    .ThenInclude(m => m.User)
                .Where(ai => ai.MemberId == memberId)
                .OrderByDescending(ai => ai.CreatedDate)
                .Take(count)
                .ToListAsync();
        }
    }
}