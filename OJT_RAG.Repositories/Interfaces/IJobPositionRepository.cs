using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IJobPositionRepository
    {
        Task<IEnumerable<JobPosition>> GetAll();
        Task<JobPosition?> GetById(long id);
        Task<long> GetNextId();
        Task Add(JobPosition entity);
        Task Update(JobPosition entity);
        Task Delete(long id);
    }
}
