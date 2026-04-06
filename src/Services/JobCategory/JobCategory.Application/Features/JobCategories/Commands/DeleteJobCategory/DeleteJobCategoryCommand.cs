using MediatR;

namespace JobCategory.Application.Features.JobCategories.Commands.DeleteJobCategory
{
    public class DeleteJobCategoryCommand : IRequest
    {
        public int Id { get; set; }
    }
}
