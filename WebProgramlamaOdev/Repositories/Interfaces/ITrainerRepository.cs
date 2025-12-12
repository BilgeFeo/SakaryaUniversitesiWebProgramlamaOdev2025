using WebProgramlamaOdev.Models;

namespace WebProgramlamaOdev.Repositories.Interfaces
{
    public interface ITrainerRepository : IRepository<Trainer>
    {
        Task<Trainer?> GetByUserIdAsync(string userId);
        Task<Trainer?> GetByUserIdWithDetailsAsync(string userId);
        Task<Trainer?> GetWithAvailabilitiesAsync(int trainerId);
        Task<Trainer?> GetWithAppointmentsAsync(int trainerId);
        Task<Trainer?> GetWithServicesAsync(int trainerId);
        Task<IEnumerable<Trainer>> GetActiveTrainersAsync();
        Task<IEnumerable<Trainer>> GetTrainersByGymAsync(int gymId);
        Task<IEnumerable<Trainer>> GetAvailableTrainersAsync(DateTime date, TimeSpan startTime, TimeSpan endTime);
        Task<bool> IsUserTrainerAsync(string userId);
    }
}

