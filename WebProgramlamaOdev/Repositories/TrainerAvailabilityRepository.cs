using Microsoft.EntityFrameworkCore;
using WebProgramlamaOdev.Data;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;

namespace WebProgramlamaOdev.Repositories
{
    public class TrainerAvailabilityRepository : Repository<TrainerAvailability>, ITrainerAvailabilityRepository
    {
        public TrainerAvailabilityRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TrainerAvailability>> GetByTrainerIdAsync(int trainerId)
        {
            return await _dbSet
                .Include(ta => ta.Trainer)
                    .ThenInclude(t => t.User)
                .Where(ta => ta.TrainerId == trainerId)
                .OrderBy(ta => ta.EventDate)
                .ThenBy(ta => ta.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<TrainerAvailability>> GetActiveByTrainerIdAsync(int trainerId)
        {
            return await _dbSet
                .Include(ta => ta.Trainer)
                    .ThenInclude(t => t.User)
                .Where(ta => ta.TrainerId == trainerId && ta.IsActive)
                .OrderBy(ta => ta.EventDate)
                .ThenBy(ta => ta.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<TrainerAvailability>> GetByTrainerAndDateAsync(
            int trainerId,
            DateTime date)
        {
            return await _dbSet
                .Include(ta => ta.Trainer)
                    .ThenInclude(t => t.User)
                .Where(ta => ta.TrainerId == trainerId &&
                            ta.EventDate.Date == date.Date &&
                            ta.IsActive)
                .OrderBy(ta => ta.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<TrainerAvailability>> GetByDateRangeAsync(
            int trainerId,
            DateTime startDate,
            DateTime endDate)
        {
            return await _dbSet
                .Include(ta => ta.Trainer)
                    .ThenInclude(t => t.User)
                .Where(ta => ta.TrainerId == trainerId &&
                            ta.EventDate >= startDate &&
                            ta.EventDate <= endDate &&
                            ta.IsActive)
                .OrderBy(ta => ta.EventDate)
                .ThenBy(ta => ta.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<TrainerAvailability>> GetAvailableSlotsAsync(
            int trainerId,
            DateTime date)
        {
            // Müsait slotları getir ve dolu randevuları filtrele
            var availabilities = await GetByTrainerAndDateAsync(trainerId, date);

            var appointments = await _context.Appointments
                .Where(a => a.TrainerId == trainerId &&
                           a.AppointmentDate.Date == date.Date &&
                           a.Status != "Cancelled")
                .ToListAsync();

            return availabilities.Where(av =>
            {
                // Bu müsaitlik için randevu var mı kontrol et
                return !appointments.Any(ap =>
                    ap.StartTime < av.EndTime && ap.EndTime > av.StartTime);
            }).ToList();
        }

        public async Task<bool> IsTrainerAvailableAsync(
            int trainerId,
            DateTime date,
            TimeSpan startTime,
            TimeSpan endTime)
        {
            return await _dbSet
                .AnyAsync(ta => ta.TrainerId == trainerId &&
                               ta.EventDate.Date == date.Date &&
                               ta.StartTime <= startTime &&
                               ta.EndTime >= endTime &&
                               ta.IsActive);
        }

        public async Task<bool> HasConflictAsync(
            int trainerId,
            DateTime date,
            TimeSpan startTime,
            TimeSpan endTime,
            int? excludeAvailabilityId = null)
        {
            var query = _dbSet
                .Where(ta => ta.TrainerId == trainerId &&
                            ta.EventDate.Date == date.Date &&
                            ta.IsActive &&
                            ((ta.StartTime < endTime && ta.EndTime > startTime)));

            if (excludeAvailabilityId.HasValue)
            {
                query = query.Where(ta => ta.Id != excludeAvailabilityId.Value);
            }

            return await query.AnyAsync();
        }
    }
}

