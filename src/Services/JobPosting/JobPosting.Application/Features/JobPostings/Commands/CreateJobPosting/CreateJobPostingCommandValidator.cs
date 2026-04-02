using FluentValidation;

namespace JobPosting.Application.Features.JobPostings.Commands.CreateJobPosting
{
    public class CreateJobPostingCommandValidator : AbstractValidator<CreateJobPostingCommand>
    {
        public CreateJobPostingCommandValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{Title} is required")
                .NotNull()
                .MaximumLength(50).WithMessage("{Title} must not exceed 50 characters.");

            RuleFor(p => p.ContactEmail)
                .NotEmpty().WithMessage("{Contact Email } is required.");

            RuleFor(p => p.JobDescription)
                .NotEmpty().WithMessage("{Job Description is required.}")
                .MaximumLength(200).WithMessage("{Job Description} should not exceed 200 characters");


        }    


    }
}
