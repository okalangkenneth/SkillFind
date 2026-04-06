using JobSeeker.Application.Models;
using MediatR;

namespace JobSeeker.Application.Features.JobSeekers.Queries.GetJobSeekerById
{
    public class GetJobSeekerByIdQuery : IRequest<JobSeekerDto>
    {
        public Guid Id { get; set; }
        public GetJobSeekerByIdQuery(Guid id) => Id = id;
    }
}
