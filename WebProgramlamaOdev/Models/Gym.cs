using System.ComponentModel.DataAnnotations;

namespace WebProgramlamaOdev.Models
{
    public class Gym
    {

        public Gym()
        {

            
        }

       
        public int Id { get; set; }

        [Required(ErrorMessage = "Bu Alan Zorunludur")]
        [StringLength(100,ErrorMessage ="Karakter Siniri Asildi")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Karakter Siniri Asildi")]
        public string Address { get; set; }

        [Phone]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }


        [EmailAddress(ErrorMessage ="Gecerli bir mail adresi olmalidir")]
        [StringLength(100,ErrorMessage ="Karakter siniri asildi")]
        public string Email { get; set; }
        
        public TimeSpan OpeningTime { get; set; }

        public TimeSpan ClosingTime { get; set; }

        public bool IsActive { get; set; } = true;
    
        List<ServiceType> ServiceList{ get; set; }
        List<Trainer> TrainerList { get; set; }
    
    
    }
}
