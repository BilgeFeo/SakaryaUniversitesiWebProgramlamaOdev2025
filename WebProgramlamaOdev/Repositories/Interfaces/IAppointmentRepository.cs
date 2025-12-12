using WebProgramlamaOdev.Models;

namespace WebProgramlamaOdev.Repositories.Interfaces
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetByMemberIdAsync(int memberId);
        Task<IEnumerable<Appointment>> GetByTrainerIdAsync(int trainerId);
        Task<IEnumerable<Appointment>> GetByStatusAsync(string status);
        Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync();
        Task<IEnumerable<Appointment>> GetPastAppointmentsAsync();
        Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Appointment>> GetMemberAppointmentsByDateRangeAsync(int memberId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Appointment>> GetTrainerAppointmentsByDateRangeAsync(int trainerId, DateTime startDate, DateTime endDate);
        Task<Appointment?> GetWithDetailsAsync(int appointmentId);
        Task<bool> HasConflictAsync(int trainerId, DateTime date, TimeSpan startTime, TimeSpan endTime, int? excludeAppointmentId = null);
        Task<IEnumerable<Appointment>> GetPendingAppointmentsAsync();
        Task<IEnumerable<Appointment>> GetTodaysAppointmentsAsync();
    }
}