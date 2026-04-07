using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JobSeeker.Infrastructure.Persistence
{
    public class JobSeekerDbContextFactory : IDesignTimeDbContextFactory<JobSeekerDbContext>
    {
        public JobSeekerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<JobSeekerDbContext>();
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=skillfind_jobseeker;Username=skillfind_user;Password=skillfind_pass");
            return new JobSeekerDbContext(optionsBuilder.Options);
        }
    }
}
