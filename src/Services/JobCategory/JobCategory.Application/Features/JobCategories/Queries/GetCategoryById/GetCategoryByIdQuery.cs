using JobCategory.Application.Models;
using MediatR;

namespace JobCategory.Application.Features.JobCategories.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<JobCategoryDto>
    {
        public int Id { get; set; }
        public GetCategoryByIdQuery(int id) => Id = id;
    }
}
