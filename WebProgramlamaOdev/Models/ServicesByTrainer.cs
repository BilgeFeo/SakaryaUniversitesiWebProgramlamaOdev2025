using System.ComponentModel.DataAnnotations;

namespace WebProgramlamaOdev.Models
{
    public class ServicesByTrainer
    {
        public int Id { get; set; }

        [Required]
        public int TrainerId { get; set; }

        [Required]
        public int ServiceTypeId { get; set; }

        // Navigation Properties
        public Trainer Trainer { get; set; }
        public ServiceType serviceType { get; set; }



    }
}
