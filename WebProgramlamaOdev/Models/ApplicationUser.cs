using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebProgramlamaOdev.ModelDtos ;
namespace WebProgramlamaOdev.Models
{
    [Table("ApplicationUser")]
    public class ApplicationUser : IdentityUser
    {

        [Key]
        int Id;

        [StringLength(50)]
        [Column("FirstName")]
        public string FirstName { get; set; }
        [StringLength(50)]
        [Column("LastName")]
        public string LastName { get; set; }

        [Required]
        [PasswordPropertyText]
        [Column("Password")]
        public string Password { get; set; }

        [EmailAddress]
        [StringLength(100)]
        [Column("Email")]
        public string Email { get; set; }

        [Phone]
        [StringLength(20)]
        [Column("PhoneNumber")]
        public string PhoneNumber { get; set; }

        
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
