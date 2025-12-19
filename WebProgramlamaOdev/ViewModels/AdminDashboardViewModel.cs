namespace WebProgramlamaOdev.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int ActiveGymCount { get; set; }
        public int ActiveTrainerCount { get; set; }
        public int ActiveServiceCount { get; set; }
        public int ActiveAppointmentCount { get; set; }
        public int ActiveMemberCount { get; set; }

        // Bonus: Toplam sayılar
        public int TotalGymCount { get; set; }
        public int TotalTrainerCount { get; set; }
        public int TotalServiceCount { get; set; }
        public int TotalAppointmentCount { get; set; }
        public int TotalMemberCount { get; set; }
    }
}