namespace WebProgramlamaOdev.ViewModels
{
    // ============================================
    // KOMBİNE VIEWMODEL - Liste + Ekleme (ANA SAYFA İÇİN)
    // ============================================
    public class TrainerServicesPageViewModel
    {
        // Mevcut hizmetler
        public List<TrainerServiceItemViewModel> CurrentServices { get; set; } = new();
        
        // Eklenebilir hizmetler (dropdown için)
        public List<AvailableServiceOption> AvailableServices { get; set; } = new();
        
        // Trainer bilgileri
        public int TrainerId { get; set; }
        public string TrainerName { get; set; }
        public string GymName { get; set; }
    }

    // ============================================
    // MEVCUT HİZMET (Listeleme için) - İSİM DEĞİŞTİRİLDİ
    // ============================================
    public class TrainerServiceItemViewModel
    {
        public int ServicesByTrainerId { get; set; }  // ServicesByTrainer Id
        public int ServiceTypeId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public int GymId { get; set; }
        public string GymName { get; set; }
        public int AppointmentCount { get; set; }  // Bu servisten kaç randevu var
    }

    // ============================================
    // EKLENEBİLİR HİZMET (Dropdown için)
    // ============================================
    public class AvailableServiceOption
    {
        public int ServiceTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        
        // Dropdown text için
        public string DisplayText => $"{Name} - {Duration} dk - {Price:N2} ₺";
    }
}
