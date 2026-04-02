using MediatR;
using System;
using System.Collections.Generic;

namespace JobPosting.Application.Features.JobPostings.Commands.UpdateJobPosting
{
    public class UpdateJobPostingCommand : IRequest
    {
        public int Id { get; set;}
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
