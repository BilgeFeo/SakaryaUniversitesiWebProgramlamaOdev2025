using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebProgramlamaOdev.Models;

[Table("Member")]
public class Member
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }

    [Required]
    public DateTime? DateOfBirth { get; set; }

    [Required]
    [StringLength(10)]
    public string Gender { get; set; }

    [Required]
    public int? Height { get; set; } // cm

    [Required]
    public decimal? Weight { get; set; } // kg

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
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<AIDailyPlanRecommendation> AIRecommendations { get; set; } = new List<AIDailyPlanRecommendation>();
}