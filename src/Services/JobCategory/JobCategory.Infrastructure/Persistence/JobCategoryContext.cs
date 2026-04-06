using Microsoft.EntityFrameworkCore;

namespace JobCategory.Infrastructure.Persistence
{
    public class JobCategoryContext : DbContext
    {
        public JobCategoryContext(DbContextOptions<JobCategoryContext> options) : base(options) { }

        public DbSet<JobCategoryEntity> Categories { get; set; }
    }
}
