using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IJobPositionRepository
    {
        Task<IEnumerable<JobPosition>> GetAllAsync();
        Task<JobPosition?> GetByIdAsync(long id);
        Task<JobPosition> AddAsync(JobPosition entity);
        Task<JobPosition> UpdateAsync(JobPosition entity);
        Task<bool> DeleteAsync(long id);
    }
}
