using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgramlamaOdev.Models
{
    public class Trainer
    {
        public Trainer()
        {
        }

        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        [Required]
        public int GymId { get; set; }

        

        [StringLength(200)]
        public string Specialization { get; set; }

        
        public bool IsActive { get; set; } = true;

        
        // Navigation Properties
        public ApplicationUser User { get; set; }
        public Gym Gym { get; set; }
        public ICollection<ServiceType> TrainerServiceTypes { get; set; }
        public ICollection<TrainerAvailability> Availabilities { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }



}

