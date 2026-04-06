using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobSeeker.Infrastructure.Persistence
{
    public class JobSeekerDbContext : IdentityDbContext<IdentityUser>
    {
        public JobSeekerDbContext(DbContextOptions<JobSeekerDbContext> options) : base(options) { }

        public DbSet<Domain.Entities.JobSeeker> JobSeekers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.UseOpenIddict();

            builder.Entity<Domain.Entities.JobSeeker>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Skills)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
            });
        }
    }
}
