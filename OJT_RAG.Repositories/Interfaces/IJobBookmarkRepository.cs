using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IJobBookmarkRepository
    {
        Task<IEnumerable<JobBookmark>> GetAllAsync();
        Task<JobBookmark?> GetByIdAsync(long id);
        Task<IEnumerable<JobBookmark>> GetByUserIdAsync(long userId);
        Task<JobBookmark> AddAsync(JobBookmark entity);
        Task<JobBookmark> UpdateAsync(JobBookmark entity);
        Task<bool> DeleteAsync(long id);
    }
}
