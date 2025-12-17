using Microsoft.EntityFrameworkCore;
using WebProgramlamaOdev.Data;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;

namespace WebProgramlamaOdev.Repositories
{
    public class ServiceTypeRepository : Repository<ServiceType>, IServiceTypeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<ServiceType> _dbSet;

        public ServiceTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<ServiceType>();
        }

        public async Task<IEnumerable<ServiceType>> GetActiveServicesAsync()
        {
            return await _dbSet
                .Include(s => s.Gym)
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceType>> GetByGymIdAsync(int gymId)
        {
            return await _dbSet
                .Include(s => s.Gym)
                .Where(s => s.GymId == gymId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceType>> GetActiveServicesByGymAsync(int gymId)
        {
            return await _dbSet
                .Include(s => s.Gym)
                .Where(s => s.GymId == gymId && s.IsActive)
                .ToListAsync();
        }

        public async Task<ServiceType?> GetWithAppointmentsAsync(int serviceId)
        {
            return await _dbSet
                .Include(s => s.Appointments)
                    .ThenInclude(a => a.Member)
                        .ThenInclude(m => m.User)
                .Include(s => s.Appointments)
                    .ThenInclude(a => a.Trainer)
                        .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(s => s.Id == serviceId);
        }

        public async Task<IEnumerable<ServiceType>> GetServicesByTrainerAsync(int trainerId)
        {
            return await _dbSet
                .Include(s => s.ServicesByTrainers)
                .Where(s => s.ServicesByTrainers.Any(st => st.TrainerId == trainerId))
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceType>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Include(s => s.Gym)
                .Where(s => s.Name.Contains(name) && s.IsActive)
                .ToListAsync();
        }


        public async Task<IEnumerable<ServiceType>> GetAllAsync()
        {
            return await _dbSet
                .Include(s => s.Gym)
                .Include(s => s.ServicesByTrainers)
                .Include(s => s.Appointments)
                .ToListAsync();
        }

        public async Task<ServiceType?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(s => s.Gym)
                .Include(s => s.ServicesByTrainers)
                .Include(s => s.Appointments)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<ServiceType?> GetWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(s => s.Gym)
                .Include(s => s.ServicesByTrainers)
                    .ThenInclude(st => st.Trainer)
                .Include(s => s.Appointments)
                    .ThenInclude(a => a.Member)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

       

        public async Task AddAsync(ServiceType serviceType)
        {
            await _dbSet.AddAsync(serviceType);
        }

        public async Task UpdateAsync(ServiceType serviceType)
        {
            _dbSet.Update(serviceType);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(ServiceType serviceType)
        {
            _dbSet.Remove(serviceType);
            await Task.CompletedTask;
        }


    }
}