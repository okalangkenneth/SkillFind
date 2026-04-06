using JobSeeker.Application.Contracts.Persistence;
using JobSeeker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobSeeker.Infrastructure.Repositories
{
    public class JobSeekerRepository : IJobSeekerRepository
    {
        private readonly JobSeekerDbContext _dbContext;

        public JobSeekerRepository(JobSeekerDbContext dbContext) => _dbContext = dbContext;

        public async Task<Domain.Entities.JobSeeker?> GetByIdAsync(Guid id) =>
            await _dbContext.JobSeekers.FindAsync(id);

        public async Task<Domain.Entities.JobSeeker?> GetByUserIdAsync(string userId) =>
            await _dbContext.JobSeekers.FirstOrDefaultAsync(j => j.UserId == userId);

        public async Task<Domain.Entities.JobSeeker> AddAsync(Domain.Entities.JobSeeker jobSeeker)
        {
            _dbContext.JobSeekers.Add(jobSeeker);
            await _dbContext.SaveChangesAsync();
            return jobSeeker;
        }

        public async Task UpdateAsync(Domain.Entities.JobSeeker jobSeeker)
        {
            _dbContext.Entry(jobSeeker).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
