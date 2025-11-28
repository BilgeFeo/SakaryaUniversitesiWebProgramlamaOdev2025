using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgramlamaOdev.Models
{
    public class Member
    {

        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Phone]
        [StringLength(20)]
       

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string Gender { get; set; } // Erkek, Kadın, Diğer

        public int? Height { get; set; } // cm

        public decimal? Weight { get; set; } // kg

        [StringLength(50)]
        public string BodyType { get; set; } // Zayıf, Normal, Kilolu, vb.


       
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
        public ApplicationUser User { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<AIDailyPlanRecommendation> AIRecommendations { get; set; }

    }
}
