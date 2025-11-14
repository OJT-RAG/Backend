using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IMajorRepository
    {
        Task<IEnumerable<Major>> GetAll();
        Task<Major?> GetById(long id);
        Task Add(Major entity);
        Task Update(Major entity);
        Task Delete(long id);
        Task<long> GetNextId();
    }
}
