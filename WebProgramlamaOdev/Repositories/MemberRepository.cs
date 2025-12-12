// Repositories/MemberRepository.cs
using Microsoft.EntityFrameworkCore;
using WebProgramlamaOdev.Data;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;

namespace WebProgramlamaOdev.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Member?> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(m => m.UserId == userId);
        }

        public async Task<Member?> GetByUserIdWithDetailsAsync(string userId)
        {
            return await _dbSet
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UserId == userId);
        }

        public async Task<Member?> GetWithAppointmentsAsync(int memberId)
        {
            return await _dbSet
                .Include(m => m.User)
                .Include(m => m.Appointments)
                    .ThenInclude(a => a.Trainer)
                        .ThenInclude(t => t.User)
                .Include(m => m.Appointments)
                    .ThenInclude(a => a.ServiceType)
                .FirstOrDefaultAsync(m => m.Id == memberId);
        }

        public async Task<Member?> GetWithAIRecommendationsAsync(int memberId)
        {
            return await _dbSet
                .Include(m => m.User)
                .Include(m => m.AIRecommendations)
                .FirstOrDefaultAsync(m => m.Id == memberId);
        }

        public async Task<IEnumerable<Member>> GetMembersWithActiveAppointmentsAsync()
        {
            return await _dbSet
                .Include(m => m.User)
                .Include(m => m.Appointments)
                .Where(m => m.Appointments.Any(a =>
                    a.Status == "Pending" ||
                    a.Status == "Confirmed"))
                .ToListAsync();
        }

        public async Task<bool> IsUserMemberAsync(string userId)
        {
            return await _dbSet.AnyAsync(m => m.UserId == userId);
        }
    }
}