using JobSeeker.Application.Contracts.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace JobSeeker.Application.Features.JobSeekers.Commands.CreateJobSeeker
{
    public class CreateJobSeekerCommandHandler : IRequestHandler<CreateJobSeekerCommand, Guid>
    {
        private readonly IJobSeekerRepository _repository;
        private readonly ILogger<CreateJobSeekerCommandHandler> _logger;

        public CreateJobSeekerCommandHandler(IJobSeekerRepository repository, ILogger<CreateJobSeekerCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateJobSeekerCommand request, CancellationToken cancellationToken)
        {
            var jobSeeker = JobSeekerEntity.Create(request.UserId, request.FirstName, request.LastName, request.Email);
            var created = await _repository.AddAsync(jobSeeker);
            _logger.LogInformation("JobSeeker profile {Id} created for user {UserId}", created.Id, created.UserId);
            return created.Id;
        }
    }
}
