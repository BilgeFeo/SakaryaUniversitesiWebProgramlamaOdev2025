using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebProgramlamaOdev.Models;

[Table("ServiceType")] // "ServieType" yerine düzeltildi
public class ServiceType
{
    [Key]
    public int Id { get; set; }

    [Required]
    
    public int GymId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    public int Duration { get; set; } // dakika

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation Properties
    [ForeignKey("GymId")]
    public Gym Gym { get; set; }
    public ICollection<ServicesByTrainer> ServicesByTrainers { get; set; } = new List<ServicesByTrainer>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}