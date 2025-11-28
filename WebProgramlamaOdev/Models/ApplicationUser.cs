using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebProgramlamaOdev.ModelDtos ;
namespace WebProgramlamaOdev.Models
{
    public class ApplicationUser : IdentityUser
    {


        int? Id;

        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string Phone { get; set; }

        
        public DateTime CreatedDate { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        // Navigation Properties
        public Trainer Trainer { get; set; }
        public Member Member { get; set; }

        //Constructors
        
        public ApplicationUser(ApplicationUserDto applicationUserDto)
        {
            this.FirstName = applicationUserDto.FirstName;
            this.LastName = applicationUserDto.LastName;
            this.Email = applicationUserDto.Email;
            this.PhoneNumber = applicationUserDto.PhoneNumber;
            this.Password = applicationUserDto.Password;
            this.CreatedDate = DateTime.Now;
        }


    }
}
