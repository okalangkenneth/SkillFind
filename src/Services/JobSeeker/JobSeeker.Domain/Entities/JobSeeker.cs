namespace JobSeeker.Domain.Entities
{
    public class JobSeeker
    {
        public Guid Id { get; private set; }
        public string UserId { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string? PhoneNumber { get; private set; }
        public string? ResumeUrl { get; private set; }
        public List<string> Skills { get; private set; } = new();
        public DateTime CreatedAt { get; private set; }

        private JobSeeker() { }

        public static JobSeeker Create(string userId, string firstName, string lastName, string email)
        {
            return new JobSeeker
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void Update(string firstName, string lastName, string? phoneNumber, string? resumeUrl, List<string> skills)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            ResumeUrl = resumeUrl;
            Skills = skills;
        }
    }
}
