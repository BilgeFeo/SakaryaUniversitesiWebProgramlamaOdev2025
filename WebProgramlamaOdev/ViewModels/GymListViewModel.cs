namespace WebProgramlamaOdev.ViewModels
{
    public class GymListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string OpeningTime { get; set; } = string.Empty;
        public string ClosingTime { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int ServiceCount { get; set; }
        public int TrainerCount { get; set; }



    }
}
