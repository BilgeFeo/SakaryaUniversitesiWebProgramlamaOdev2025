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
        [ForeignKey("Trainer")]
        public int TrainerId { get; set; }

        [Required]
        [ForeignKey("ServiceType")]
        public int ServiceTypeId { get; set; }

        // Navigation Properties
        public Trainer Trainer { get; set; }
        public ServiceType serviceType { get; set; }



    }
}
