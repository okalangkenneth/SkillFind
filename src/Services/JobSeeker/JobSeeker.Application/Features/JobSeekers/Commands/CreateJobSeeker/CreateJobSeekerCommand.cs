using MediatR;

namespace JobSeeker.Application.Features.JobSeekers.Commands.CreateJobSeeker
{
    public class CreateJobSeekerCommand : IRequest<Guid>
    {
        public string UserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
