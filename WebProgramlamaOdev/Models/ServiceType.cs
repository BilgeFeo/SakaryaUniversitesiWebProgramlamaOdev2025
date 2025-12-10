using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgramlamaOdev.Models
{
    [Table("ServieType")]
    public class ServiceType
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Gym")]
        public int GymId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("Name")]
        public string Name { get; set; }

        [StringLength(500)]
        [Column("Description")]
        public string Description { get; set; }

        [Required]
        [Column("Duration")]
        public int Duration { get; set; } // dakika

        [Required]
        [Column("Price",TypeName = "decimal(18,2)")]
        
        public decimal Price { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public Gym Gym { get; set; }
        public ICollection<ServicesByTrainer> servicesByTrainers{ get; set; }
        public ICollection<Appointment> Appointments { get; set; }

    }
}
