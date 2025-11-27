using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgramlamaOdev.Models
{
    public class TrainerAvailability
    {

        public int Id { get; set; }

        [Required]
        public int TrainerId { get; set; }

        [Required]
        [Range(0, 6)] // 0=Pazartesi, 6=Pazar
        public int DayOfWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Property
        public Trainer Trainer { get; set; }

        [NotMapped]
        public string DayName
        {
            get
            {
                return DayOfWeek switch
                {
                    0 => "Pazartesi",
                    1 => "Salı",
                    2 => "Çarşamba",
                    3 => "Perşembe",
                    4 => "Cuma",
                    5 => "Cumartesi",
                    6 => "Pazar",
                    _ => "Bilinmiyor"
                };
            }




        }
    }
}
