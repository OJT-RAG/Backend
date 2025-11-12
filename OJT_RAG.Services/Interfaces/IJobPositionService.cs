using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Services.Interfaces
{
    public interface IJobPositionService
    {
        Task<IEnumerable<JobPosition>> GetAllAsync();
        Task<JobPosition?> GetByIdAsync(long id);
        Task<JobPosition> CreateAsync(JobPosition request);
        Task<JobPosition?> UpdateAsync(long id, JobPosition request);
        Task<bool> DeleteAsync(long id);
    }
}
