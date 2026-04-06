using JobCategory.Application.Contracts.Persistence;
using JobCategory.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace JobCategory.Application.Features.JobCategories.Commands.DeleteJobCategory
{
    public class DeleteJobCategoryCommandHandler : IRequestHandler<DeleteJobCategoryCommand>
    {
        private readonly IJobCategoryRepository _repository;
        private readonly ILogger<DeleteJobCategoryCommandHandler> _logger;

        public DeleteJobCategoryCommandHandler(IJobCategoryRepository repository, ILogger<DeleteJobCategoryCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(DeleteJobCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Domain.Entities.JobCategory), request.Id);

            await _repository.DeleteAsync(category);
            _logger.LogInformation("JobCategory {Id} deleted.", request.Id);
        }
    }
}
