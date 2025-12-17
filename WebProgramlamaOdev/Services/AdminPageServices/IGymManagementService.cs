using WebProgramlamaOdev.DTOs;

namespace WebProgramlamaOdev.Services.AdminPageServices
{
    public interface IGymManagementService
    {

        Task<bool> CreateGymAndSaveOnDatabase(CreateGymRequestDto GymModelInstance);
    }
}
