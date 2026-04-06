using FluentValidation;

namespace JobSeeker.Application.Features.JobSeekers.Commands.CreateJobSeeker
{
    public class CreateJobSeekerCommandValidator : AbstractValidator<CreateJobSeekerCommand>
    {
        public CreateJobSeekerCommandValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(p => p.LastName).NotEmpty().MaximumLength(100);
            RuleFor(p => p.Email).NotEmpty().EmailAddress();
            RuleFor(p => p.UserId).NotEmpty();
        }
    }
}
