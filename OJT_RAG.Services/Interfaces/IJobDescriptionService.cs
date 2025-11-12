
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Services.Interfaces
{
    public interface IJobDescriptionService
    {
        Task<IEnumerable<JobDescription>> GetAll();
        Task<JobDescription?> GetById(int id);
        Task<JobDescription> Create(JobDescription model);
        Task<JobDescription> Update(JobDescription model);
        Task<bool> Delete(int id);
    }
}
