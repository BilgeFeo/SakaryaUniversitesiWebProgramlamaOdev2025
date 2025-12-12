using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgramlamaOdev.Models
{
    [Table("Gyms")]
    public class Gym
    {
        [Key]
        public int Id { get; set; }

        
        [Required]
        [MaxLength(450)]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;

       
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        [Required]
        public TimeSpan OpeningTime { get; set; }

        [Required]
        public TimeSpan ClosingTime { get; set; }

        public bool IsActive { get; set; } = true;

       
        public ICollection<ServiceType> ServiceList { get; set; } = new List<ServiceType>();
        public ICollection<Trainer> TrainerList { get; set; } = new List<Trainer>();

        
        [NotMapped]
        public string Email => User?.Email ?? string.Empty;

        [NotMapped]
        public string PhoneNumber => User?.PhoneNumber ?? string.Empty;
    }
}