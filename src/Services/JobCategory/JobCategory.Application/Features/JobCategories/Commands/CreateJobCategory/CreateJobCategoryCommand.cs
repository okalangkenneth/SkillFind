using MediatR;

namespace JobCategory.Application.Features.JobCategories.Commands.CreateJobCategory
{
    public class CreateJobCategoryCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
