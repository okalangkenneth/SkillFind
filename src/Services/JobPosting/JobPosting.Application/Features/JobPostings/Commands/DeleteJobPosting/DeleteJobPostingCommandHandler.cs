using JobPosting.Application.Contracts.Persistence;
using JobPosting.Application.Exceptions;
using JobPosting.Application.Interfaces;
using JobPosting.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace JobPosting.Application.Features.JobPostings.Commands.DeleteJobPosting
{
    public class DeleteJobPostingCommandHandler : IRequestHandler<DeleteJobPostingCommand>
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly ILogger<DeleteJobPostingCommandHandler> _logger;
        private readonly ICurrentUserService _currentUser;

        public DeleteJobPostingCommandHandler(
            IJobPostingRepository jobPostingRepository,
            ILogger<DeleteJobPostingCommandHandler> logger,
            ICurrentUserService currentUser)
        {
            _jobPostingRepository = jobPostingRepository;
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task Handle(DeleteJobPostingCommand request, CancellationToken cancellationToken)
        {
            var jobPostingToDelete = await _jobPostingRepository.GetByIdAsync(request.Id);
            if (jobPostingToDelete == null)
                throw new NotFoundException(nameof(Job_Posting), request.Id);

            // Only enforce ownership for posts that have an employer stamp
            if (!string.IsNullOrEmpty(jobPostingToDelete.EmployerId) &&
                !jobPostingToDelete.IsOwnedBy(_currentUser.UserId))
                throw new ForbiddenException("You can only delete job posts that belong to your account.");

            await _jobPostingRepository.DeleteAsync(jobPostingToDelete);
            _logger.LogInformation("Job Posting {Id} successfully deleted.", jobPostingToDelete.Id);
        }
    }
}
