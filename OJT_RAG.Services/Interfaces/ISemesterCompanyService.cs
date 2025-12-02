using OJT_RAG.DTOs.SemesterCompanyDTO;
using OJT_RAG.ModelView.SemesterCompanyModelView;

namespace OJT_RAG.Services.Interfaces
{
    public interface ISemesterCompanyService
    {
        Task<IEnumerable<SemesterCompanyModelView>> GetAll();
        Task<SemesterCompanyModelView?> GetById(long id);
        Task<IEnumerable<SemesterCompanyModelView>> GetBySemester(long semesterId);
        Task<IEnumerable<SemesterCompanyModelView>> GetByCompany(long companyId);

        Task<bool> Create(CreateSemesterCompanyDTO dto);
        Task<bool> Update(UpdateSemesterCompanyDTO dto);
        Task<bool> Delete(long id);
    }
}
