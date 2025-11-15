using OJT_RAG.Repositories.Entities;
using OJT_RAG.Services.DTOs.JobDescription;

namespace OJT_RAG.Services.Interfaces
{
    public interface IJobDescriptionService
    {
        Task<IEnumerable<JobDescription>> GetAllAsync();
        Task<JobDescription?> GetByIdAsync(long id);
        Task<JobDescription> CreateAsync(CreateJobDescriptionDTO dto);
        Task<JobDescription?> UpdateAsync(UpdateJobDescriptionDTO dto);
        Task<bool> DeleteAsync(long id);
    }
}
