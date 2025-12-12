using WebProgramlamaOdev.Models;

namespace WebProgramlamaOdev.Repositories.Interfaces
{
    public interface ITrainerAvailabilityRepository : IRepository<TrainerAvailability>
    {
        Task<IEnumerable<TrainerAvailability>> GetByTrainerIdAsync(int trainerId);
        Task<IEnumerable<TrainerAvailability>> GetActiveByTrainerIdAsync(int trainerId);
        Task<IEnumerable<TrainerAvailability>> GetByTrainerAndDateAsync(int trainerId, DateTime date);
        Task<IEnumerable<TrainerAvailability>> GetByDateRangeAsync(int trainerId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<TrainerAvailability>> GetAvailableSlotsAsync(int trainerId, DateTime date);
        Task<bool> IsTrainerAvailableAsync(int trainerId, DateTime date, TimeSpan startTime, TimeSpan endTime);
        Task<bool> HasConflictAsync(int trainerId, DateTime date, TimeSpan startTime, TimeSpan endTime, int? excludeAvailabilityId = null);
    }
}

