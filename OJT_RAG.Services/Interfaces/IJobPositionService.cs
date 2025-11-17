using OJT_RAG.DTOs.JobPositionDTO;
using OJT_RAG.ModelView.JobPositionModelView;

namespace OJT_RAG.Services.Interfaces
{
    public interface IJobPositionService
    {
        Task<IEnumerable<JobPositionModelView>> GetAll();
        Task<JobPositionModelView?> GetById(long id);
        Task<bool> Create(CreateJobPositionDTO dto);
        Task<bool> Update(UpdateJobPositionDTO dto);
        Task<bool> Delete(long id);
    }
}
