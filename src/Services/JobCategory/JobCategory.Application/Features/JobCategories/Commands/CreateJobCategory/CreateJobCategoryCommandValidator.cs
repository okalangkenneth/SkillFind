using FluentValidation;

namespace JobCategory.Application.Features.JobCategories.Commands.CreateJobCategory
{
    public class CreateJobCategoryCommandValidator : AbstractValidator<CreateJobCategoryCommand>
    {
        public CreateJobCategoryCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{Name} is required.")
                .MaximumLength(100).WithMessage("{Name} must not exceed 100 characters.");

            RuleFor(p => p.Description)
                .MaximumLength(500).WithMessage("{Description} must not exceed 500 characters.");
        }
    }
}
