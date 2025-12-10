using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgramlamaOdev.Models
{
    [Table("TrainerAvailability")]
    public class TrainerAvailability
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Trainer")]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }
        [Required]
        [Column("EventDate")]
        public DateTime EventDate { get; set; }

        [Required]
        [Column("StartTime")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Column("EndTime")]
        public TimeSpan EndTime { get; set; }

        [Required]
        [Column("IsActive")]
        public bool IsActive { get; set; } = true;


        

        
       
    }
}
