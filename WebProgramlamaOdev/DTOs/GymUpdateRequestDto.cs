using System.ComponentModel.DataAnnotations;

namespace WebProgramlamaOdev.DTOs
{
    public class GymUpdateRequestDto
    {


        public int Id { get; set; }

        [Required(ErrorMessage = "Spor salonu adı zorunludur")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Adres zorunludur")]
        [StringLength(250)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Açılış saati zorunludur")]
        [DataType(DataType.Time)]
        public TimeSpan OpeningTime { get; set; }

        [Required(ErrorMessage = "Kapanış saati zorunludur")]
        [DataType(DataType.Time)]
        public TimeSpan ClosingTime { get; set; }

        public bool IsActive { get; set; }






    }
}
