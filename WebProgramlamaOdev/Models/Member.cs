using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgramlamaOdev.Models
{
    [Table("Member")]
    public class Member
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }


        [Required]
        [Column("DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        [Column("Gender")]
        public string Gender { get; set; } // Erkek, Kadın, Diğer

        [Required]
        [Column("Gender")]
        public int? Height { get; set; } // cm

        [Required]
        [Column("Gender")]
        public decimal? Weight { get; set; } // kg


        [Column("Age")]
        [NotMapped]
        public int? Age
        {
            get
            {
                if (DateOfBirth.HasValue)
                {
                    var today = DateTime.Today;
                    var age = today.Year - DateOfBirth.Value.Year;
                    if (DateOfBirth.Value.Date > today.AddYears(-age)) age--;
                    return age;
                }
                return null;
            }
        }

        // Navigation Properties
        
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<AIDailyPlanRecommendation> AIRecommendations { get; set; }

    }
}
