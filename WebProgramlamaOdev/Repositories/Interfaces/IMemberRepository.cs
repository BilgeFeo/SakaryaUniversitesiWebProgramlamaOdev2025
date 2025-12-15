using WebProgramlamaOdev.Models;

namespace WebProgramlamaOdev.Repositories.Interfaces
{
    public interface IMemberRepository : IRepository<Member>
    {
        Task<Member?> GetByUserIdAsync(string userId);
        Task<Member?> GetByUserIdWithDetailsAsync(string userId);
        Task<Member?> GetWithAppointmentsAsync(int memberId);
        Task<Member?> GetWithAIRecommendationsAsync(int memberId);
        Task<IEnumerable<Member>> GetMembersWithActiveAppointmentsAsync();
        Task<bool> IsUserMemberAsync(string userId);

        Task<bool> AddAsync(Member member);
    }
}