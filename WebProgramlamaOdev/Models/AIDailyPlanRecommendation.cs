using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgramlamaOdev.Models
{
    public class AIDailyPlanRecommendation
    {

        public int Id { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Required]
        [StringLength(50)]
        public string RequestType { get; set; } // Exercise, Diet, BodyPrediction

        [Column(TypeName = "nvarchar(MAX)")]
        public string InputData { get; set; } // JSON formatında

        [Column(TypeName = "nvarchar(MAX)")]
        public string AIResponse { get; set; } // JSON formatında

        [StringLength(500)]
        public string GeneratedImagePath { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Property
        public Member Member { get; set; }



    }
}
