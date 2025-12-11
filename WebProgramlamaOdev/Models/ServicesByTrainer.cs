using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProgramlamaOdev.Models
{
    [Table("ServicesByTrainer")]
    public class ServicesByTrainer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        
        public int TrainerId { get; set; }

        [Required]
        public int ServiceTypeId { get; set; }

        // Navigation Properties
        [ForeignKey("TrainerId")]
        public Trainer Trainer { get; set; }
        
        [ForeignKey("ServiceTypeId")]
        public ServiceType serviceType { get; set; }



    }
}
