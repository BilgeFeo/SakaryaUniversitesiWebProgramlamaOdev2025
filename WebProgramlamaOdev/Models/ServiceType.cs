using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgramlamaOdev.Models
{
    public class ServiceType
    {
        public int Id { get; set; }

        public int GymId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int Duration { get; set; } // dakika

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public Gym Gym { get; set; }
        public ICollection<ServicesByTrainer> servicesByTrainers{ get; set; }
        public ICollection<Appointment> Appointments { get; set; }

    }
}
