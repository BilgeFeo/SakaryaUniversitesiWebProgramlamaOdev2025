using System.ComponentModel.DataAnnotations;

namespace WebProgramlamaOdev.DTOs
{
    public class CreateServiceRequestDto
    {
        [Required(ErrorMessage = "Spor salonu seçimi zorunludur")]
        public int GymId { get; set; }

        [Required(ErrorMessage = "Servis adı zorunludur")]
        [StringLength(100, ErrorMessage = "Servis adı en fazla 100 karakter olabilir")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Süre zorunludur")]
        [Range(1, 480, ErrorMessage = "Süre 1 ile 480 dakika arasında olmalıdır")]
        public int Duration { get; set; } // dakika

        [Required(ErrorMessage = "Fiyat zorunludur")]
        [Range(0.01, 999999.99, ErrorMessage = "Fiyat 0.01 ile 999999.99 arasında olmalıdır")]
        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
