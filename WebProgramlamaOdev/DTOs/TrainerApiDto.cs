namespace WebProgramlamaOdev.DTOs
{
    public class TrainerApiDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Specialization { get; set; }
        public string GymName { get; set; }
        public int ActiveClientCount { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}