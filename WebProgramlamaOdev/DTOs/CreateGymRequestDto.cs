using System.ComponentModel.DataAnnotations;

namespace WebProgramlamaOdev.DTOs
{
    public class CreateGymRequestDto
    {
        [Required(ErrorMessage = "Spor salonu adı zorunludur")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Adres zorunludur")]
        [StringLength(250)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Açılış saati zorunludur")]
        [DataType(DataType.Time)]
        public TimeSpan OpeningTime { get; set; } = new TimeSpan(6, 0, 0);

        [Required(ErrorMessage = "Kapanış saati zorunludur")]
        [DataType(DataType.Time)]
        public TimeSpan ClosingTime { get; set; } = new TimeSpan(23, 0, 0);






    }
}
