using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgramlamaOdev.Models
{
    public class ApplicationUser : IdentityUser
    {
        // IdentityUser zaten Id, Email, PhoneNumber, PasswordHash sağlıyor
        // Bu yüzden tekrar tanımlamaya gerek yok

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        // Navigation Properties
        public Trainer? Trainer { get; set; }
        public Member? Member { get; set; }
    }
}