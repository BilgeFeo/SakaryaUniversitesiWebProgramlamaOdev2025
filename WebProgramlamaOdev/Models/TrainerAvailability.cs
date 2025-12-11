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
        
        public int TrainerId { get; set; }

        
        [ForeignKey("Trainer")]
        public Trainer Trainer { get; set; }
        
        [Required]
        
        public DateTime EventDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;


        

        
       
    }
}
