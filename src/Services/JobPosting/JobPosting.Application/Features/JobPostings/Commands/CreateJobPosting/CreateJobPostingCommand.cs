using JobPosting.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPosting.Application.Features.JobPostings.Commands.CreateJobPosting
{
    public class CreateJobPostingCommand : IRequest<int>
    {
        public string Title { get; set; }
        public string JobDescription { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string JobLocation { get; set; }
        public decimal Salary { get; set; }
        public bool IsRemote { get; set; }
        public bool IsFullTime { get; set; }
        public string ContactEmail { get; set; }

    }
}
