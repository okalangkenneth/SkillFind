namespace JobSeeker.Application.Contracts.Persistence
{
    public interface IJobSeekerRepository
    {
        Task<JobSeekerEntity?> GetByIdAsync(Guid id);
        Task<JobSeekerEntity?> GetByUserIdAsync(string userId);
        Task<JobSeekerEntity> AddAsync(JobSeekerEntity jobSeeker);
        Task UpdateAsync(JobSeekerEntity jobSeeker);
    }
}
