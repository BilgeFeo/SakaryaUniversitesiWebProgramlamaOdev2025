using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgramlamaOdev.Models
{
    public class ApplicationUser : IdentityUser
    {
       
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

      

        [MaxLength(20)]
        public string UserType { get; set; } = "Member"; // Member, Trainer, Gym

        [NotMapped]
        public string DisplayName
        {
            get
            {
                return UserType switch
                {
                    "Member" => $"{FirstName} {LastName}".Trim(),
                    "Trainer" => $"{FirstName} {LastName}".Trim(),
                    "Gym" => Gym?.Name ?? "Salon",
                    _ => Email ?? "Kullanıcı"
                };
            }
        }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}".Trim();

        public Member? Member { get; set; }
        public Trainer? Trainer { get; set; }

        
        public Gym? Gym { get; set; }
    }
}