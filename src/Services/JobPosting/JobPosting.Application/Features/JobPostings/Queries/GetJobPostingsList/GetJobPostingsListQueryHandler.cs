using JobPosting.Application.Contracts.Persistence;
using Mapster;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JobPosting.Application.Features.JobPostings.Queries.GetJobPostingsList
{
    public class GetJobPostingsListQueryHandler : IRequestHandler<GetJobPostingsListQuery, List<JobPostingsVm>>
    {
        private readonly IJobPostingRepository _jobPostingRepository;

        public GetJobPostingsListQueryHandler(IJobPostingRepository jobPostingRepository)
        {
            _jobPostingRepository = jobPostingRepository;
        }

        public async Task<List<JobPostingsVm>> Handle(GetJobPostingsListQuery request, CancellationToken cancellationToken)
        {
            var jobPostingList = await _jobPostingRepository.GetJobPostingsByTitle(request.Title);
            return jobPostingList.Adapt<List<JobPostingsVm>>();
        }
    }
}
