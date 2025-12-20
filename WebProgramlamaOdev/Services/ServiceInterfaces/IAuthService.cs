using WebProgramlamaOdev.Models;

namespace WebProgramlamaOdev.Services.ServiceInterfaces
{
    public interface IAuthService
    {
        Task<ApplicationUser> GetUserByEmailAsync(string LoginAttemptEmail);
        
        Task<string> ValidateUser(string LoginAttemptEmail, string LoginAttemptPassword);
        Task<bool> ValidateAdmin(string LoginAttemptEmail, string LoginAttemptPassword);


    }
}
