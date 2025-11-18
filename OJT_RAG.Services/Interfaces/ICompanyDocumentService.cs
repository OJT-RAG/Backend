using OJT_RAG.DTOs.CompanyDocumentDTO;
using OJT_RAG.ModelView.CompanyDocumentModelView;

namespace OJT_RAG.Services.Interfaces
{
    public interface ICompanyDocumentService
    {
        Task<IEnumerable<CompanyDocumentModelView>> GetAll();
        Task<CompanyDocumentModelView?> GetById(long id);
        Task<IEnumerable<CompanyDocumentModelView>> GetBySemester(long semId);
        Task<bool> Create(CreateCompanyDocumentDTO dto);
        Task<bool> Update(UpdateCompanyDocumentDTO dto);
        Task<bool> Delete(long id);
    }
}
