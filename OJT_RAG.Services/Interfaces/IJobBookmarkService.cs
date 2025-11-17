using OJT_RAG.DTOs.JobBookmarkDTO;
using OJT_RAG.ModelView.JobBookmarkModelView;

namespace OJT_RAG.Services.Interfaces
{
    public interface IJobBookmarkService
    {
        Task<IEnumerable<JobBookmarkModelView>> GetAll();
        Task<JobBookmarkModelView?> GetById(long id);
        Task<IEnumerable<JobBookmarkModelView>> GetByUserId(long userId);
        Task<bool> Create(CreateJobBookmarkDTO dto);
        Task<bool> Update(UpdateJobBookmarkDTO dto);
        Task<bool> Delete(long id);
    }
}
