using JobCategory.Application.Contracts.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace JobCategory.Application.Features.JobCategories.Commands.CreateJobCategory
{
    public class CreateJobCategoryCommandHandler : IRequestHandler<CreateJobCategoryCommand, int>
    {
        private readonly IJobCategoryRepository _repository;
        private readonly ILogger<CreateJobCategoryCommandHandler> _logger;

        public CreateJobCategoryCommandHandler(IJobCategoryRepository repository, ILogger<CreateJobCategoryCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<int> Handle(CreateJobCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = JobCategoryEntity.Create(request.Name, request.Description);
            var created = await _repository.AddAsync(category);
            _logger.LogInformation("JobCategory {Id} created: {Name}", created.Id, created.Name);
            return created.Id;
        }
    }
}
