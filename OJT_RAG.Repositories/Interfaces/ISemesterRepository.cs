using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface ISemesterRepository
    {
        Task<IEnumerable<Semester>> GetAll();
        Task<Semester?> GetById(long id);
        Task<long> GetNextId();
        Task Add(Semester entity);
        Task Update(Semester entity);
        Task Delete(long id);
    }
}
