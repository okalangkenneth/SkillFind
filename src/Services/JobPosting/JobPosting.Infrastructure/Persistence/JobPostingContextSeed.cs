using JobPosting.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPosting.Infrastructure.Persistence
{
    public class JobPostingContextSeed
    {
        public static async Task SeedAsync(JobPostingContext jobPostingContext, ILogger<JobPostingContextSeed> logger)
        {
            if (!jobPostingContext.Job_Postings.Any())
            {
                jobPostingContext.Job_Postings.AddRange(GetPreconfiguredJobPostings());
                await jobPostingContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(JobPostingContext).Name);

            }
        }

        private static IEnumerable<Job_Posting> GetPreconfiguredJobPostings()
        {
            return new List<Job_Posting>

            {
                new Job_Posting() {Title = ".NET Developer",ContactEmail = "okalang.ok@gmail.com",JobDescription= "Backend development with 3 yrs experince" }
            };
        }

    }
}
