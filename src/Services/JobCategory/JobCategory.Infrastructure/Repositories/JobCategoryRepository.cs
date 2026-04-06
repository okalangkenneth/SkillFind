using JobCategory.Application.Contracts.Persistence;
using JobCategory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobCategory.Infrastructure.Repositories
{
    public class JobCategoryRepository : RepositoryBase<JobCategoryEntity>, IJobCategoryRepository
    {
        public JobCategoryRepository(JobCategoryContext dbContext) : base(dbContext) { }

        public async Task<bool> ExistsAsync(string name) =>
            await _dbContext.Categories.AnyAsync(c => c.Name == name);
    }
}
