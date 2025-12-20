namespace WebProgramlamaOdev.ViewModels
{
    public class TrainerDashboardViewModel
    {
        public int TrainerId { get; set; }
        public string TrainerName { get; set; }
        public string GymName { get; set; }
        public string Specialization { get; set; }
        public bool IsActive { get; set; }

        // İstatistikler
        public int TotalServices { get; set; }
        public int TotalAppointments { get; set; }
        public int PendingAppointments { get; set; }
        public int TodayAppointments { get; set; }
        public int ConfirmedAppointments { get; set; }

        // Yaklaşan randevular
        public List<UpcomingAppointmentViewModel> UpcomingAppointments { get; set; } = new();
    }

    public class UpcomingAppointmentViewModel
    {
        public int Id { get; set; }
        public string MemberName { get; set; }
        public string ServiceName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
    }
}
