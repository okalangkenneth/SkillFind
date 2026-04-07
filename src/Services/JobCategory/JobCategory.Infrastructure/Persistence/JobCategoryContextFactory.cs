using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JobCategory.Infrastructure.Persistence
{
    public class JobCategoryContextFactory : IDesignTimeDbContextFactory<JobCategoryContext>
    {
        public JobCategoryContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<JobCategoryContext>();
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=skillfind_jobcategory;Username=skillfind_user;Password=skillfind_pass");
            return new JobCategoryContext(optionsBuilder.Options);
        }
    }
}
