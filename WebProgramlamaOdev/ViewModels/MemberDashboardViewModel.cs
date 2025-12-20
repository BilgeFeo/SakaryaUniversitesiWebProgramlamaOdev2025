using System;

namespace WebProgramlamaOdev.ViewModels
{
    public class MemberDashboardViewModel
    {
        // Kişisel Bilgiler
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }

        // Fiziksel Bilgiler
        public DateTime? DateOfBirth { get; set; }
        public int Age { get; set; }
        public int? Height { get; set; } // cm
        public decimal? Weight { get; set; } // kg

        // İstatistikler (Opsiyonel ama hoş durur)
        public int ActiveAppointmentsCount { get; set; }
        public int AIProgramsCount { get; set; }

        // Computed Properties (Hesaplanmış Özellikler)
        public string HeightDisplay => Height.HasValue ? $"{Height} cm" : "-";
        public string WeightDisplay => Weight.HasValue ? $"{Weight} kg" : "-";

        // Vücut Kitle İndeksi (BMI) Hesabı
        public double BMI
        {
            get
            {
                if (Height.HasValue && Weight.HasValue && Height > 0)
                {
                    double heightInMeters = Height.Value / 100.0;
                    return (double)(Weight.Value / (decimal)(heightInMeters * heightInMeters));
                }
                return 0;
            }
        }

        public string BMIStatus
        {
            get
            {
                if (BMI == 0) return "Belirsiz";
                if (BMI < 18.5) return "Zayıf";
                if (BMI < 25) return "Normal";
                if (BMI < 30) return "Fazla Kilolu";
                return "Obez";
            }
        }

        public string BMIBadgeClass
        {
            get
            {
                if (BMI == 0) return "bg-secondary";
                if (BMI < 18.5) return "bg-info";
                if (BMI < 25) return "bg-success";
                if (BMI < 30) return "bg-warning";
                return "bg-danger";
            }
        }
    }
}