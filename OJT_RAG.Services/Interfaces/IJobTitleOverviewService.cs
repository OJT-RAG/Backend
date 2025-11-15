using OJT_RAG.DTOs.JobTitleOverviewDTO;
using OJT_RAG.ModelView.JobTitleOverviewModelView;

namespace OJT_RAG.Services.Interfaces
{
    public interface IJobTitleOverviewService
    {
        Task<IEnumerable<JobTitleOverviewModelView>> GetAll();
        Task<JobTitleOverviewModelView?> GetById(long id);
        Task<bool> Create(CreateJobTitleOverviewDTO dto);
        Task<bool> Update(UpdateJobTitleOverviewDTO dto);
        Task<bool> Delete(long id);
    }
}
