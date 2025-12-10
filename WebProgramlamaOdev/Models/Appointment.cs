using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgramlamaOdev.Models
{
    [Table("Appointment")]
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("MemberId")]
        public int MemberId { get; set; }


        [Required]
        [ForeignKey("TrainerId")]
        public int TrainerId { get; set; }

        [Required]
        [ForeignKey("ServiceTypeId")]
        public int ServiceTypeId { get; set; }

        [Required]
        [Column("AppointmentDate ")]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [Column("StartTime")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Column("EndTime")]
        public TimeSpan EndTime { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Status")]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled, Completed

        [Required]
        [Column( "TotalPrice")]
        public decimal TotalPrice { get; set; }
        [Column("CreatedDate")]
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        [Column("IsConfirmed")]
        public bool IsConfirmed { get; set; } = false;

        [NotMapped]
        public DateTime AppointmentDateTime => AppointmentDate.Date + StartTime;

        // Navigation Properties
        
        public Member? Member { get; set; }
        public Trainer? Trainer { get; set; }
        
        public ServiceType? ServiceType { get; set; }


    }
}
