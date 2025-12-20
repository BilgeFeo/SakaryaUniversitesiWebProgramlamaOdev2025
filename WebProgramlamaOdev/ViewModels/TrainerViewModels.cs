namespace WebProgramlamaOdev.ViewModels
{
    // ============================================
    // HİZMETLER İÇİN VIEWMODEL
    // ============================================
    public class TrainerServiceViewModel
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
    // HİZMET EKLEME İÇİN VIEWMODEL
    // ============================================
    public class AddServiceViewModel
    {
        public int TrainerId { get; set; }
        public List<AvailableServiceViewModel> AvailableServices { get; set; } = new();
    }

    public class AvailableServiceViewModel
    {
        public int ServiceTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public bool IsAlreadyAdded { get; set; }
    }

    // ============================================
    // RANDEVULAR İÇİN VIEWMODEL
    // ============================================
    public class TrainerAppointmentViewModel
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberEmail { get; set; }
        public string MemberPhone { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
