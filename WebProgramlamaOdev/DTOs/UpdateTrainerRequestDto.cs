using System.ComponentModel.DataAnnotations;

namespace WebProgramlamaOdev.DTOs
{
    public class UpdateTrainerRequestDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Spor salonu seçimi zorunludur")]
        public int GymId { get; set; }

        // ApplicationUser fields
        [Required(ErrorMessage = "Ad zorunludur")]
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur")]
        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        public string PhoneNumber { get; set; }

        // Trainer fields
        [Required(ErrorMessage = "Uzmanlık alanı zorunludur")]
        [StringLength(200, ErrorMessage = "Uzmanlık alanı en fazla 200 karakter olabilir")]
        public string Specialization { get; set; }

        public bool IsActive { get; set; }
    }
}
