using System.ComponentModel.DataAnnotations;

namespace WebProgramlamaOdev.ViewModels
{
    public class MemberProfileUpdateViewModel
    {
        // --- Kişisel Bilgiler (ApplicationUser) ---
        [Display(Name = "Ad")]
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        public string LastName { get; set; }

        [Display(Name = "E-posta")]
        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Telefon")]
        [Phone]
        public string PhoneNumber { get; set; }

        // --- Fiziksel Bilgiler (Member) ---
        [Display(Name = "Boy (cm)")]
        [Range(100, 250, ErrorMessage = "Geçerli bir boy giriniz.")]
        public int? Height { get; set; }

        [Display(Name = "Kilo (kg)")]
        [Range(30, 300, ErrorMessage = "Geçerli bir kilo giriniz.")]
        public decimal? Weight { get; set; }

        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Cinsiyet")]
        public string Gender { get; set; }

        // --- Şifre Değiştirme (Opsiyonel) ---
        // Kullanıcı şifre değiştirmek istemiyorsa buraları boş bırakabilir.

        [Display(Name = "Mevcut Şifre")]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [Display(Name = "Yeni Şifre")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string? NewPassword { get; set; }

        [Display(Name = "Yeni Şifre (Tekrar)")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Şifreler uyuşmuyor.")]
        public string? ConfirmNewPassword { get; set; }
    }
}