// Repositories/TrainerRepository.cs
using Microsoft.EntityFrameworkCore;
using WebProgramlamaOdev.Data;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;

namespace WebProgramlamaOdev.Repositories
{
    public class TrainerRepository : Repository<Trainer>, ITrainerRepository
    {
        public TrainerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Trainer?> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(t => t.UserId == userId);
        }

        public async Task<Trainer?> GetByUserIdWithDetailsAsync(string userId)
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.Gym)
                .FirstOrDefaultAsync(t => t.UserId == userId);
        }

        public async Task<Trainer?> GetWithAvailabilitiesAsync(int trainerId)
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.Availabilities)
                .FirstOrDefaultAsync(t => t.Id == trainerId);
        }

        public async Task<Trainer?> GetWithAppointmentsAsync(int trainerId)
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.Appointments)
                    .ThenInclude(a => a.Member)
                        .ThenInclude(m => m.User)
                .Include(t => t.Appointments)
                    .ThenInclude(a => a.ServiceType)
                .FirstOrDefaultAsync(t => t.Id == trainerId);
        }

        public async Task<Trainer?> GetWithServicesAsync(int trainerId)
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.TrainerServiceTypes)
                .FirstOrDefaultAsync(t => t.Id == trainerId);
        }

        public async Task<IEnumerable<Trainer>> GetActiveTrainersAsync()
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.Gym)
                .Where(t => t.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Trainer>> GetTrainersByGymAsync(int gymId)
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.Gym)
                .Where(t => t.GymId == gymId && t.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Trainer>> GetAvailableTrainersAsync(
            DateTime date,
            TimeSpan startTime,
            TimeSpan endTime)
        {
            var trainersWithAvailability = await _dbSet
                .Include(t => t.User)
                .Include(t => t.Gym)
                .Include(t => t.Availabilities)
                .Include(t => t.Appointments)
                .Where(t => t.IsActive)
                .ToListAsync();

            return trainersWithAvailability.Where(t =>
            {
                // Müsaitlik kontrolü
                var hasAvailability = t.Availabilities.Any(a =>
                    a.EventDate.Date == date.Date &&
                    a.StartTime <= startTime &&
                    a.EndTime >= endTime &&
                    a.IsActive);

                if (!hasAvailability)
                    return false;

                // Randevu çakışması kontrolü
                var hasConflict = t.Appointments.Any(a =>
                    a.AppointmentDate.Date == date.Date &&
                    a.Status != "Cancelled" &&
                    ((a.StartTime < endTime && a.EndTime > startTime)));

                return !hasConflict;
            }).ToList();
        }

        public async Task<bool> IsUserTrainerAsync(string userId)
        {
            return await _dbSet.AnyAsync(t => t.UserId == userId);
        }
    }
}
