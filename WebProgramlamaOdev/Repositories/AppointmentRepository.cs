using Microsoft.EntityFrameworkCore;
using WebProgramlamaOdev.Data;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;

namespace WebProgramlamaOdev.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Appointment>> GetByMemberIdAsync(int memberId)
        {
            return await _dbSet
                .Include(a => a.Member)
                    .ThenInclude(m => m.User)
                .Include(a => a.Trainer)
                    .ThenInclude(t => t.User)
                .Include(a => a.ServiceType)
                .Where(a => a.MemberId == memberId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByTrainerIdAsync(int trainerId)
        {
            return await _dbSet
                .Include(a => a.Member)
                    .ThenInclude(m => m.User)
                .Include(a => a.Trainer)
                    .ThenInclude(t => t.User)
                .Include(a => a.ServiceType)
                .Where(a => a.TrainerId == trainerId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Include(a => a.Member)
                    .ThenInclude(m => m.User)
                .Include(a => a.Trainer)
                    .ThenInclude(t => t.User)
                .Include(a => a.ServiceType)
                .Where(a => a.Status == status)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync()
        {
            var now = DateTime.Now;
            return await _dbSet
                .Include(a => a.Member)
                    .ThenInclude(m => m.User)
                .Include(a => a.Trainer)
                    .ThenInclude(t => t.User)
                .Include(a => a.ServiceType)
                .Where(a => a.AppointmentDate >= now.Date &&
                           (a.Status == "Pending" || a.Status == "Confirmed"))
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetPastAppointmentsAsync()
        {
            var now = DateTime.Now;
            return await _dbSet
                .Include(a => a.Member)
                    .ThenInclude(m => m.User)
                .Include(a => a.Trainer)
                    .ThenInclude(t => t.User)
                .Include(a => a.ServiceType)
                .Where(a => a.AppointmentDate < now.Date)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByDateRangeAsync(
            DateTime startDate,
            DateTime endDate)
        {
            return await _dbSet
                .Include(a => a.Member)
                    .ThenInclude(m => m.User)
                .Include(a => a.Trainer)
                    .ThenInclude(t => t.User)
                .Include(a => a.ServiceType)
                .Where(a => a.AppointmentDate >= startDate && a.AppointmentDate <= endDate)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetMemberAppointmentsByDateRangeAsync(
            int memberId,
            DateTime startDate,
            DateTime endDate)
        {
            return await _dbSet
                .Include(a => a.Member)
                    .ThenInclude(m => m.User)
                .Include(a => a.Trainer)
                    .ThenInclude(t => t.User)
                .Include(a => a.ServiceType)
                .Where(a => a.MemberId == memberId &&
                           a.AppointmentDate >= startDate &&
                           a.AppointmentDate <= endDate)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetTrainerAppointmentsByDateRangeAsync(
            int trainerId,
            DateTime startDate,
            DateTime endDate)
        {
            return await _dbSet
                .Include(a => a.Member)
                    .ThenInclude(m => m.User)
                .Include(a => a.Trainer)
                    .ThenInclude(t => t.User)
                .Include(a => a.ServiceType)
                .Where(a => a.TrainerId == trainerId &&
                           a.AppointmentDate >= startDate &&
                           a.AppointmentDate <= endDate)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<Appointment?> GetWithDetailsAsync(int appointmentId)
        {
            return await _dbSet
                .Include(a => a.Member)
                    .ThenInclude(m => m.User)
                .Include(a => a.Trainer)
                    .ThenInclude(t => t.User)
                .Include(a => a.Trainer)
                    .ThenInclude(t => t.Gym)
                .Include(a => a.ServiceType)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);
        }

        public async Task<bool> HasConflictAsync(
            int trainerId,
            DateTime date,
            TimeSpan startTime,
            TimeSpan endTime,
            int? excludeAppointmentId = null)
        {
            var query = _dbSet
                .Where(a => a.TrainerId == trainerId &&
                           a.AppointmentDate.Date == date.Date &&
                           a.Status != "Cancelled" &&
                           ((a.StartTime < endTime && a.EndTime > startTime)));

            if (excludeAppointmentId.HasValue)
            {
                query = query.Where(a => a.Id != excludeAppointmentId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<Appointment>> GetPendingAppointmentsAsync()
        {
            return await _dbSet
                .Include(a => a.Member)
                    .ThenInclude(m => m.User)
                .Include(a => a.Trainer)
                    .ThenInclude(t => t.User)
                .Include(a => a.ServiceType)
                .Where(a => a.Status == "Pending")
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetTodaysAppointmentsAsync()
        {
            var today = DateTime.Today;
            return await _dbSet
                .Include(a => a.Member)
                    .ThenInclude(m => m.User)
                .Include(a => a.Trainer)
                    .ThenInclude(t => t.User)
                .Include(a => a.ServiceType)
                .Where(a => a.AppointmentDate.Date == today &&
                           (a.Status == "Pending" || a.Status == "Confirmed"))
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }
    }
}

