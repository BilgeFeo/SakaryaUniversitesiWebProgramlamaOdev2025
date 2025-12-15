using WebProgramlamaOdev.Models;

namespace WebProgramlamaOdev.Services.ServiceInterfaces
{
    public interface IAuthService
    {

        Task<Dictionary<string, ApplicationUser>> GetAllUsersMapAsync();
        Task<string> ValidateUser(string LoginAttemptEmail, string LoginAttemptPassword);
        Task<bool> ValidateAdmin(string LoginAttemptEmail, string LoginAttemptPassword);


    }
}
