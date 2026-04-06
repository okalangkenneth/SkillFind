using JobCategory.Application.Contracts.Persistence;
using JobCategory.Domain.Common;
using JobCategory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobCategory.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
    {
        protected readonly JobCategoryContext _dbContext;

        public RepositoryBase(JobCategoryContext dbContext) => _dbContext = dbContext;

        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync() =>
            await _dbContext.Set<T>().ToListAsync();

        public async Task<T?> GetByIdAsync(int id) =>
            await _dbContext.Set<T>().FindAsync(id);

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate) =>
            await _dbContext.Set<T>().Where(predicate).ToListAsync();
    }
}
