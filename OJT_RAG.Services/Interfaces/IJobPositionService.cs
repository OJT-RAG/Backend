using OJT_RAG.DTOs.JobPositionDTO;
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Services.Interfaces
{
    public interface IJobPositionService
    {
        Task<IEnumerable<JobPosition>> GetAllAsync();
        Task<JobPosition?> GetByIdAsync(long id);
        Task<JobPosition> CreateAsync(JobPositionCreateDTO dto);
        Task<JobPosition?> UpdateAsync(long id, JobPositionUpdateDTO dto);
        Task<bool> DeleteAsync(long id);
    }
}
