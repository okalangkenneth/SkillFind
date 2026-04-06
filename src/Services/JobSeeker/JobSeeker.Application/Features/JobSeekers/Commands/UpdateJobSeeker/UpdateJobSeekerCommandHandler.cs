using JobSeeker.Application.Contracts.Persistence;
using JobSeeker.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace JobSeeker.Application.Features.JobSeekers.Commands.UpdateJobSeeker
{
    public class UpdateJobSeekerCommandHandler : IRequestHandler<UpdateJobSeekerCommand>
    {
        private readonly IJobSeekerRepository _repository;
        private readonly ILogger<UpdateJobSeekerCommandHandler> _logger;

        public UpdateJobSeekerCommandHandler(IJobSeekerRepository repository, ILogger<UpdateJobSeekerCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(UpdateJobSeekerCommand request, CancellationToken cancellationToken)
        {
            var jobSeeker = await _repository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Domain.Entities.JobSeeker), request.Id);

            jobSeeker.Update(request.FirstName, request.LastName, request.PhoneNumber, request.ResumeUrl, request.Skills);
            await _repository.UpdateAsync(jobSeeker);
            _logger.LogInformation("JobSeeker profile {Id} updated.", request.Id);
        }
    }
}
