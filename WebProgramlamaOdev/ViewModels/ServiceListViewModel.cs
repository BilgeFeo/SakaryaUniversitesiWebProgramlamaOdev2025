namespace WebProgramlamaOdev.ViewModels
{
    public class ServiceListViewModel
    {
        public int Id { get; set; }
        public int GymId { get; set; }
        public string GymName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; } // dakika
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public int TrainerCount { get; set; } // Bu servisi veren antrenör sayısı
        public int AppointmentCount { get; set; } // Bu servis için randevu sayısı
    }
}
