using JobCategory.Application.Contracts.Persistence;
using JobCategory.Application.Exceptions;
using JobCategory.Application.Models;
using Mapster;
using MediatR;

namespace JobCategory.Application.Features.JobCategories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, JobCategoryDto>
    {
        private readonly IJobCategoryRepository _repository;

        public GetCategoryByIdQueryHandler(IJobCategoryRepository repository) => _repository = repository;

        public async Task<JobCategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(JobCategoryEntity), request.Id);
            return category.Adapt<JobCategoryDto>();
        }
    }
}
