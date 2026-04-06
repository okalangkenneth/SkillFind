using JobSeeker.Application.Contracts.Persistence;
using JobSeeker.Application.Exceptions;
using JobSeeker.Application.Models;
using Mapster;
using MediatR;

namespace JobSeeker.Application.Features.JobSeekers.Queries.GetJobSeekerById
{
    public class GetJobSeekerByIdQueryHandler : IRequestHandler<GetJobSeekerByIdQuery, JobSeekerDto>
    {
        private readonly IJobSeekerRepository _repository;

        public GetJobSeekerByIdQueryHandler(IJobSeekerRepository repository) => _repository = repository;

        public async Task<JobSeekerDto> Handle(GetJobSeekerByIdQuery request, CancellationToken cancellationToken)
        {
            var jobSeeker = await _repository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Domain.Entities.JobSeeker), request.Id);
            return jobSeeker.Adapt<JobSeekerDto>();
        }
    }
}
