using WebProgramlamaOdev.Models;

namespace WebProgramlamaOdev.Repositories.Interfaces
{
    public interface IServicesByTrainerRepository : IRepository<ServicesByTrainer>
    {
        Task<IEnumerable<ServicesByTrainer>> GetByTrainerIdAsync(int trainerId);
        Task<IEnumerable<ServicesByTrainer>> GetByServiceTypeIdAsync(int serviceTypeId);
        Task<ServicesByTrainer?> GetByTrainerAndServiceAsync(int trainerId, int serviceTypeId);
        Task<bool> TrainerHasServiceAsync(int trainerId, int serviceTypeId);
        Task AddTrainerServiceAsync(int trainerId, int serviceTypeId);
        Task RemoveTrainerServiceAsync(int trainerId, int serviceTypeId);
    }
}

