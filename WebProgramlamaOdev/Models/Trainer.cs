using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgramlamaOdev.Models
{
    [Table("Trainer")]
    public class Trainer
    {
        public Trainer()
        {
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        [ForeignKey("Gym")]
        public int GymId { get; set; }
        public Gym Gym { get; set; }

        [Required]
        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        
        // Navigation Properties
        
        
        public ICollection<ServiceType> TrainerServiceTypes { get; set; }
        public ICollection<TrainerAvailability> Availabilities { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }



}

