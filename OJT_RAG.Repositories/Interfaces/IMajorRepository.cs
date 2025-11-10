using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IMajorRepository
    {
        Task<IEnumerable<Major>> GetAllAsync();
        Task<Major?> GetByIdAsync(long id);
        Task<Major> AddAsync(Major major);
        Task<Major> UpdateAsync(Major major);
        Task<bool> DeleteAsync(long id);
    }
}
