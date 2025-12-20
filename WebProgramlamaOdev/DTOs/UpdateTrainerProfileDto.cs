using System.ComponentModel.DataAnnotations;

namespace WebProgramlamaOdev.DTOs
{
    public class UpdateTrainerProfileDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Ad zorunludur")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        public string PhoneNumber { get; set; }

        [StringLength(200)]
        public string? Specialization { get; set; }

        public bool IsActive { get; set; }

        // ✅ Read-only display - NOT REQUIRED
        public int GymId { get; set; }
        public string? GymName { get; set; }  // ✅ Nullable ve Required yok
    }
}
