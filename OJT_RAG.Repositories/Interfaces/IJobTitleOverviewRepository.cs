using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IJobTitleOverviewRepository
    {
        Task<IEnumerable<JobTitleOverview>> GetAllAsync();
        Task<JobTitleOverview?> GetByIdAsync(long id);
        Task<JobTitleOverview> AddAsync(JobTitleOverview entity);
        Task<JobTitleOverview> UpdateAsync(JobTitleOverview entity);
        Task<bool> DeleteAsync(long id);
    }
}
