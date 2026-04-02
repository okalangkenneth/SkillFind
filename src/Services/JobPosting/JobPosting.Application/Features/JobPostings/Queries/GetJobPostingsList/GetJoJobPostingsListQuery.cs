using MediatR;
using System.Collections.Generic;

namespace JobPosting.Application.Features.JobPostings.Queries.GetJobPostingsList
{
    public class GetJobPostingsListQuery : IRequest<List<JobPostingsVm>>
    {
        public string Title { get; set; }


        public GetJobPostingsListQuery(string title)
        {
            Title = title;
        }
    }
}
