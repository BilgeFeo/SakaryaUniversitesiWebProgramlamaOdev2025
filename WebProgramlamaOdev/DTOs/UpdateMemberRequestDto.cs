using System.ComponentModel.DataAnnotations;

namespace WebProgramlamaOdev.DTOs
{
    public class UpdateMemberRequestDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

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

        // Member fields
        [Required(ErrorMessage = "Doğum tarihi zorunludur")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Cinsiyet seçimi zorunludur")]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Boy bilgisi zorunludur")]
        [Range(50, 250, ErrorMessage = "Boy 50-250 cm arasında olmalıdır")]
        public int Height { get; set; }

        [Required(ErrorMessage = "Kilo bilgisi zorunludur")]
        [Range(20, 300, ErrorMessage = "Kilo 20-300 kg arasında olmalıdır")]
        public decimal Weight { get; set; }
    }
}
