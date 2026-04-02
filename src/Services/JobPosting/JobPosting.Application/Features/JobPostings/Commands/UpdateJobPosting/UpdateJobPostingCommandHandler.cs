using JobPosting.Application.Contracts.Persistence;
using JobPosting.Application.Exceptions;
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

        public UpdateJobPostingCommandHandler(IJobPostingRepository jobPostingRepository, ILogger<UpdateJobPostingCommandHandler> logger)
        {
            _jobPostingRepository = jobPostingRepository;
            _logger = logger;
        }

        public async Task Handle(UpdateJobPostingCommand request, CancellationToken cancellationToken)
        {
            var jobPostingToUpdate = await _jobPostingRepository.GetByIdAsync(request.Id);
            if (jobPostingToUpdate == null)
                throw new NotFoundException(nameof(Job_Posting), request.Id);

            request.Adapt(jobPostingToUpdate);

            await _jobPostingRepository.UpdateAsync(jobPostingToUpdate);

            _logger.LogInformation("Job Posting {Id} successfully updated.", jobPostingToUpdate.Id);
        }
    }
}
