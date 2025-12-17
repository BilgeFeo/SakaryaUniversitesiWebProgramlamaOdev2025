

// Repositories/Interfaces/IServiceTypeRepository.cs
using WebProgramlamaOdev.Models;

namespace WebProgramlamaOdev.Repositories.Interfaces
{
    public interface IServiceTypeRepository : IRepository<ServiceType>
    {
        Task<IEnumerable<ServiceType>> GetActiveServicesAsync();
        Task<IEnumerable<ServiceType>> GetByGymIdAsync(int gymId);
        Task<IEnumerable<ServiceType>> GetActiveServicesByGymAsync(int gymId);
        Task<ServiceType?> GetWithAppointmentsAsync(int serviceId);
        Task<IEnumerable<ServiceType>> GetServicesByTrainerAsync(int trainerId);
        Task<IEnumerable<ServiceType>> SearchByNameAsync(string name);
        Task<IEnumerable<ServiceType>> GetAllAsync();
        Task<ServiceType?> GetByIdAsync(int id);
        Task<ServiceType?> GetWithDetailsAsync(int id);
        Task AddAsync(ServiceType serviceType);
        Task UpdateAsync(ServiceType serviceType);
        Task DeleteAsync(ServiceType serviceType);






    }
}

