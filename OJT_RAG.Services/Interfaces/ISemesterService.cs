using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Services.Interfaces
{
    public interface ISemesterService
    {
        Task<IEnumerable<Semester>> GetAllAsync();
        Task<Semester?> GetByIdAsync(long id);
        Task<Semester> CreateAsync(Semester request);
        Task<Semester?> UpdateAsync(long id, Semester request);
        Task<bool> DeleteAsync(long id);
    }
}
