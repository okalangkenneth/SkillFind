using JobPosting.Application.Contracts.Persistence;
using JobPosting.Application.Exceptions;
using JobPosting.Application.Interfaces;
using JobPosting.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace JobPosting.Application.Features.JobPostings.Commands.UpdateJobPosting
{
    public class UpdateJobPostingCommandHandler : IRequestHandler<UpdateJobPostingCommand>
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly ILogger<UpdateJobPostingCommandHandler> _logger;
        private readonly ICurrentUserService _currentUser;

        public UpdateJobPostingCommandHandler(
            IJobPostingRepository jobPostingRepository,
            ILogger<UpdateJobPostingCommandHandler> logger,
            ICurrentUserService currentUser)
        {
            _jobPostingRepository = jobPostingRepository;
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task Handle(UpdateJobPostingCommand request, CancellationToken cancellationToken)
        {
            var jobPostingToUpdate = await _jobPostingRepository.GetByIdAsync(request.Id);
            if (jobPostingToUpdate == null)
                throw new NotFoundException(nameof(Job_Posting), request.Id);

            // Only enforce ownership for posts that have an employer stamp
            if (!string.IsNullOrEmpty(jobPostingToUpdate.EmployerId) &&
                !jobPostingToUpdate.IsOwnedBy(_currentUser.UserId))
                throw new ForbiddenException("You can only update job posts that belong to your account.");

            request.Adapt(jobPostingToUpdate);

            await _jobPostingRepository.UpdateAsync(jobPostingToUpdate);

            _logger.LogInformation("Job Posting {Id} successfully updated.", jobPostingToUpdate.Id);
        }
    }
}
