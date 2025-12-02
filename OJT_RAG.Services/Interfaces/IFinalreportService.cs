using OJT_RAG.DTOs.FinalreportDTO;
using OJT_RAG.ModelView.FinalreportModelView;

namespace OJT_RAG.Services.Interfaces
{
    public interface IFinalreportService
    {
        Task<IEnumerable<FinalreportModelView>> GetAll();
        Task<FinalreportModelView?> GetById(long id);
        Task<IEnumerable<FinalreportModelView>> GetByUser(long userId);
        Task<IEnumerable<FinalreportModelView>> GetBySemester(long semesterId);
        Task<IEnumerable<FinalreportModelView>> GetByJob(long jobPositionId);

        Task<bool> Create(CreateFinalreportDTO dto);
        Task<bool> Update(UpdateFinalreportDTO dto);
        Task<bool> Delete(long id);
    }
}
