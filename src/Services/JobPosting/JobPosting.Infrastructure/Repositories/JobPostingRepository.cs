using JobPosting.Application.Contracts.Persistence;
using JobPosting.Domain.Entities;
using JobPosting.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobPosting.Infrastructure.Repositories
{
    public class JobPostingRepository : RepositoryBase<Job_Posting>, IJobPostingRepository
    {

        public JobPostingRepository(JobPostingContext dbContext) : base(dbContext)
        {

        }


        public async Task<IEnumerable<Job_Posting>> GetJobPostingsByTitle(string title)
        {
            var jobPostingList = await _dbContext.Job_Postings
                                .Where(t => t.Title == title)
                                .ToListAsync();
            return jobPostingList;
        }
    }
}
