using System;

namespace WebProgramlamaOdev.ViewModels
{
    /// <summary>
    /// Admin panelinde randevuları listelemek için kullanılır
    /// </summary>
    public class AppointmentListViewModel
    {
        public int Id { get; set; }
        
        // Member Bilgileri
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberEmail { get; set; }
        public string MemberPhone { get; set; }
        
        // Trainer Bilgileri
        public int TrainerId { get; set; }
        public string TrainerName { get; set; }
        public string TrainerSpecialization { get; set; }
        
        // Service Bilgileri
        public int ServiceTypeId { get; set; }
        public string ServiceName { get; set; }
        public decimal ServicePrice { get; set; }
        
        // Randevu Bilgileri
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        
        // Durum Bilgileri
        public string Status { get; set; }
        public bool IsConfirmed { get; set; }
        public decimal TotalPrice { get; set; }
        
        // Ek Bilgiler
        public DateTime CreatedDate { get; set; }
        
        // Computed Properties
        public string StatusBadgeClass => Status switch
        {
            "Pending" => "badge bg-warning text-dark",
            "Confirmed" => "badge bg-success",
            "Cancelled" => "badge bg-danger",
            "Completed" => "badge bg-info",
            _ => "badge bg-secondary"
        };
        
        public string StatusText => Status switch
        {
            "Pending" => "Beklemede",
            "Confirmed" => "Onaylandı",
            "Cancelled" => "İptal Edildi",
            "Completed" => "Tamamlandı",
            _ => Status
        };
        
        public bool CanConfirm => Status == "Pending";
        public bool CanCancel => Status == "Pending" || Status == "Confirmed";
        public bool CanDelete => Status == "Cancelled" || Status == "Pending";
        
        public string FormattedDate => AppointmentDate.ToString("dd MMMM yyyy, dddd");
        public string FormattedTime => $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
        public string FormattedDateTime => $"{FormattedDate} {FormattedTime}";
    }
}
