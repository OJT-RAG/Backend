
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IJobTitleOverviewRepository
    {
        Task<IEnumerable<JobTitleOverview>> GetAll();
        Task<JobTitleOverview?> GetById(long id);
        Task<JobTitleOverview> Add(JobTitleOverview model);
        Task<JobTitleOverview> Update(JobTitleOverview model);
        Task<bool> Delete(int id);
    }
}
