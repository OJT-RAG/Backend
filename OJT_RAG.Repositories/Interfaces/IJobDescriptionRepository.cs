
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IJobDescriptionRepository
    {
        Task<IEnumerable<JobDescription>> GetAll();
        Task<JobDescription?> GetById(long id);
        Task<JobDescription> Add(JobDescription entity);
        Task<JobDescription> Update(JobDescription entity);
        Task<bool> Delete(int id);
    }
}
