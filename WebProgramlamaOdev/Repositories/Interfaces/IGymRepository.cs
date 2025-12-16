using WebProgramlamaOdev.Models;

namespace WebProgramlamaOdev.Repositories.Interfaces
{
    public interface IGymRepository : IRepository<Gym>
    {
        Task<IEnumerable<Gym>> GetActiveGymsAsync();
        Task<Gym?> GetWithServicesAsync(int gymId);
        Task<Gym?> GetWithTrainersAsync(int gymId);
        Task<Gym?> GetWithServicesAndTrainersAsync(int gymId);
        Task<IEnumerable<Gym>> SearchByNameAsync(string name);
        Task<bool> IsGymActiveAsync(int gymId);

        Task<bool> AddAsync(Gym gym);
    }
}

