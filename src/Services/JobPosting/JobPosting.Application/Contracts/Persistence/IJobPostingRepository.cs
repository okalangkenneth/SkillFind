using JobPosting.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPosting.Application.Contracts.Persistence
{
    public interface IJobPostingRepository : IAsyncRepository<Job_Posting>
    {
        Task<IEnumerable<Job_Posting>> GetJobPostingsByTitle(string title);
    }
}
