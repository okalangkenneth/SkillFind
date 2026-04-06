using JobCategory.Application.Contracts.Persistence;
using JobCategory.Application.Models;
using Mapster;
using MediatR;

namespace JobCategory.Application.Features.JobCategories.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<JobCategoryDto>>
    {
        private readonly IJobCategoryRepository _repository;

        public GetAllCategoriesQueryHandler(IJobCategoryRepository repository) => _repository = repository;

        public async Task<List<JobCategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _repository.GetAllAsync();
            return categories.Adapt<List<JobCategoryDto>>();
        }
    }
}
