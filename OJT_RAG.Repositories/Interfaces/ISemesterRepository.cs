using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface ISemesterRepository
    {
        Task<IEnumerable<Semester>> GetAllAsync();
        Task<Semester?> GetByIdAsync(long id);
        Task<Semester> AddAsync(Semester entity);
        Task<Semester> UpdateAsync(Semester entity);
        Task<bool> DeleteAsync(long id);
    }
}
