using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgramlamaOdev.Models
{
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
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsConfirmed { get; set; } = false;

        [NotMapped]
        public DateTime AppointmentDateTime => AppointmentDate.Date + StartTime;

        // Navigation Properties
        public Member Member { get; set; }
        public Trainer Trainer { get; set; }
        public ServiceType ServiceType { get; set; }


    }
}
