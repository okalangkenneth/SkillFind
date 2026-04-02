using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPosting.Application.Features.JobPostings.Commands.UpdateJobPosting
{
   public class UpdateJobPostingCommandValidator: AbstractValidator<UpdateJobPostingCommand>
    {
        public UpdateJobPostingCommandValidator()
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
