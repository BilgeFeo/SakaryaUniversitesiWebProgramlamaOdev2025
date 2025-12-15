using WebProgramlamaOdev.Models;


namespace WebProgramlamaOdev.Repositories.Interfaces
{
    public interface IApplicationUserRepository
    {

        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<bool> AddAsync(ApplicationUser user);
    }
}
