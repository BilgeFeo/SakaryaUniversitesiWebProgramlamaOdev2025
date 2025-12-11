using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgramlamaOdev.Models
{
    [Table("Appointment")]
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public int MemberId { get; set; }


        [Required]
        
        public int TrainerId { get; set; }

        [Required]
        public int ServiceTypeId { get; set; }

        [Required]
     
        public DateTime AppointmentDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled, Completed

        [Required]
        public decimal TotalPrice { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public bool IsConfirmed { get; set; } = false;

        [NotMapped]
        public DateTime AppointmentDateTime => AppointmentDate.Date + StartTime;

        // Navigation Properties

        [ForeignKey("MemberId")]
        public Member? Member { get; set; }
        [ForeignKey("TrainerId")]
        public Trainer? Trainer { get; set; }
        [ForeignKey("ServiceTypeId")]
        public ServiceType? ServiceType { get; set; }


    }
}
