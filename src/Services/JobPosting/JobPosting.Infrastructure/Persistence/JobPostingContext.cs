using JobPosting.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobPosting.Infrastructure.Persistence
{
    public class JobPostingContext : DbContext
    {
        public JobPostingContext(DbContextOptions<JobPostingContext> options) : base(options)
        {
        }

        public DbSet<Job_Posting> Job_Postings { get; set; }
    }
}
