using MediatR;

namespace JobSeeker.Application.Features.JobSeekers.Commands.UpdateJobSeeker
{
    public class UpdateJobSeekerCommand : IRequest
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? ResumeUrl { get; set; }
        public List<string> Skills { get; set; } = new();
    }
}
