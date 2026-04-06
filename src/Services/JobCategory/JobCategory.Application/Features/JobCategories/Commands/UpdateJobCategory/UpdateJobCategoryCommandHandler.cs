using JobCategory.Application.Contracts.Persistence;
using JobCategory.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace JobCategory.Application.Features.JobCategories.Commands.UpdateJobCategory
{
    public class UpdateJobCategoryCommandHandler : IRequestHandler<UpdateJobCategoryCommand>
    {
        private readonly IJobCategoryRepository _repository;
        private readonly ILogger<UpdateJobCategoryCommandHandler> _logger;

        public UpdateJobCategoryCommandHandler(IJobCategoryRepository repository, ILogger<UpdateJobCategoryCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(UpdateJobCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Domain.Entities.JobCategory), request.Id);

            category.Update(request.Name, request.Description);
            await _repository.UpdateAsync(category);
            _logger.LogInformation("JobCategory {Id} updated.", request.Id);
        }
    }
}
