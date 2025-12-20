using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebProgramlamaOdev.ViewModels
{
    public class CreateAppointmentViewModel
    {
        [Required(ErrorMessage = "Lütfen bir antrenör seçiniz.")]
        [Display(Name = "Antrenör")]
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Lütfen bir hizmet seçiniz.")]
        [Display(Name = "Hizmet")]
        public int ServiceTypeId { get; set; }

        [Required(ErrorMessage = "Lütfen tarih seçiniz.")]
        [DataType(DataType.Date)]
        [Display(Name = "Randevu Tarihi")]
        public DateTime AppointmentDate { get; set; } = DateTime.Today.AddDays(1);

        [Required(ErrorMessage = "Lütfen başlangıç saati seçiniz.")]
        [DataType(DataType.Time)]
        [Display(Name = "Başlangıç Saati")]
        public TimeSpan StartTime { get; set; }

        // Dropdown Listeleri İçin
        public List<SelectListItem> Trainers { get; set; } = new();

        // Ekranda Bilgi Göstermek İçin (Post işleminde gerekli değil)
        public string? SelectedTrainerName { get; set; }
        public string? SelectedServiceName { get; set; }
    }
}