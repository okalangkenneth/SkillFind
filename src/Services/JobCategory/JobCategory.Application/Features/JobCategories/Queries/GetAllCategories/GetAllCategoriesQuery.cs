using JobCategory.Application.Models;
using MediatR;

namespace JobCategory.Application.Features.JobCategories.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<List<JobCategoryDto>> { }
}
