using Microsoft.EntityFrameworkCore;
using WebProgramlamaOdev.Data;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;

namespace WebProgramlamaOdev.Repositories
{
    public class ServicesByTrainerRepository : Repository<ServicesByTrainer>, IServicesByTrainerRepository
    {
        public ServicesByTrainerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ServicesByTrainer>> GetByTrainerIdAsync(int trainerId)
        {
            return await _dbSet
                .Include(st => st.Trainer)
                    .ThenInclude(t => t.User)
                .Include(st => st.serviceType)
                .Where(st => st.TrainerId == trainerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ServicesByTrainer>> GetByServiceTypeIdAsync(int serviceTypeId)
        {
            return await _dbSet
                .Include(st => st.Trainer)
                    .ThenInclude(t => t.User)
                .Include(st => st.serviceType)
                .Where(st => st.ServiceTypeId == serviceTypeId)
                .ToListAsync();
        }

        public async Task<ServicesByTrainer?> GetByTrainerAndServiceAsync(
            int trainerId,
            int serviceTypeId)
        {
            return await _dbSet
                .Include(st => st.Trainer)
                    .ThenInclude(t => t.User)
                .Include(st => st.serviceType)
                .FirstOrDefaultAsync(st => st.TrainerId == trainerId &&
                                          st.ServiceTypeId == serviceTypeId);
        }

        public async Task<bool> TrainerHasServiceAsync(int trainerId, int serviceTypeId)
        {
            return await _dbSet
                .AnyAsync(st => st.TrainerId == trainerId && st.ServiceTypeId == serviceTypeId);
        }

        public async Task AddTrainerServiceAsync(int trainerId, int serviceTypeId)
        {
            var exists = await TrainerHasServiceAsync(trainerId, serviceTypeId);
            if (!exists)
            {
                var serviceByTrainer = new ServicesByTrainer
                {
                    TrainerId = trainerId,
                    ServiceTypeId = serviceTypeId
                };
                await AddAsync(serviceByTrainer);
            }
        }

        public async Task RemoveTrainerServiceAsync(int trainerId, int serviceTypeId)
        {
            var service = await GetByTrainerAndServiceAsync(trainerId, serviceTypeId);
            if (service != null)
            {
                await DeleteAsync(service);
            }
        }
    }
}

