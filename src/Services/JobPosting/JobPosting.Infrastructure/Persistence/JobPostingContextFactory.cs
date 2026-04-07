using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JobPosting.Infrastructure.Persistence
{
    public class JobPostingContextFactory : IDesignTimeDbContextFactory<JobPostingContext>
    {
        public JobPostingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<JobPostingContext>();
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=skillfind_jobposting;Username=skillfind_user;Password=skillfind_pass");
            return new JobPostingContext(optionsBuilder.Options);
        }
    }
}
