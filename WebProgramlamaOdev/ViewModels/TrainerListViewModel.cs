namespace WebProgramlamaOdev.ViewModels
{
    public class TrainerListViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int GymId { get; set; }
        public string GymName { get; set; }
        
        // ApplicationUser fields
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        
        // Trainer fields
        public string Specialization { get; set; }
        public bool IsActive { get; set; }
        
        // Statistics
        public int ServiceCount { get; set; } // TrainerServiceTypes sayısı
        public int AppointmentCount { get; set; } // Appointments sayısı
    }
}
