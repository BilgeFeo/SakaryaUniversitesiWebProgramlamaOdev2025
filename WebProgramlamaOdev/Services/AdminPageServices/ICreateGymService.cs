using WebProgramlamaOdev.DTOs;

namespace WebProgramlamaOdev.Services.AdminPageServices
{
    public interface ICreateGymService
    {

        Task<bool> CreateGymAndSaveOnDatabase(CreateGymRequestDto GymModelInstance);
    }
}
