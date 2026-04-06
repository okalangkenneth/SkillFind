namespace JobSeeker.Application.Models
{
    public class JobSeekerDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? ResumeUrl { get; set; }
        public List<string> Skills { get; set; } = new();
    }
}
