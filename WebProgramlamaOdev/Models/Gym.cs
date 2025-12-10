using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgramlamaOdev.Models
{
    [Table("Gym")]
    public class Gym
    {

        public Gym()
        {

            
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Bu Alan Zorunludur")]
        [StringLength(100,ErrorMessage ="Karakter Siniri Asildi")]
        [Column("Name")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Karakter Siniri Asildi")]
        [Column("Address")]
        public string Address { get; set; }

        [Phone]
        [MaxLength(20)]
        [Column("PhoneNumber")]
        public string PhoneNumber { get; set; }


        [EmailAddress(ErrorMessage ="Gecerli bir mail adresi olmalidir")]
        [StringLength(100,ErrorMessage ="Karakter siniri asildi")]
        [Column("Email")]
        public string Email { get; set; }
        
        public TimeSpan OpeningTime { get; set; }

        public TimeSpan ClosingTime { get; set; }
        [Column("IsActive")]
        public bool IsActive { get; set; } = true;
    
        List<ServiceType> ServiceList{ get; set; }
        List<Trainer> TrainerList { get; set; }
    
    
    }
}
