namespace JobCategory.Application.Contracts.Persistence
{
    public interface IJobCategoryRepository : IAsyncRepository<JobCategoryEntity>
    {
        Task<bool> ExistsAsync(string name);
    }
}
