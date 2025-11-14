
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Services.Interfaces
{
    public interface IJobTitleOverviewService
    {
        Task<IEnumerable<JobTitleOverview>> GetAll();
        Task<JobTitleOverview?> GetById(int id);
        Task<JobTitleOverview> Create(JobTitleOverview model);
        Task<JobTitleOverview> Update(JobTitleOverview model);
        Task<bool> Delete(int id);
    }
}
