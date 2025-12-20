namespace WebProgramlamaOdev.ViewModels
{
    public class MemberListViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        
        // ApplicationUser fields
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        
        // Member fields
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public int? Height { get; set; }
        public decimal? Weight { get; set; }
        
        // Calculated
        public int? Age
        {
            get
            {
                if (DateOfBirth.HasValue)
                {
                    var today = DateTime.Today;
                    var age = today.Year - DateOfBirth.Value.Year;
                    if (DateOfBirth.Value.Date > today.AddYears(-age)) age--;
                    return age;
                }
                return null;
            }
        }
        
        // Statistics
        public int AppointmentCount { get; set; }
        public int AIRecommendationCount { get; set; }
    }
}
