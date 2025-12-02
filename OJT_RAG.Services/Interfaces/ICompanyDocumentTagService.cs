using OJT_RAG.DTOs.CompanyDocumentTag;
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Services.Interfaces
{
    public interface ICompanyDocumentTagService
    {
        Task<IEnumerable<Companydocumenttag>> GetAll();
        Task<Companydocumenttag?> GetById(long id);
        Task<bool> Create(CreateCompanyDocumentTagDTO dto);
        Task<bool> Update(UpdateCompanyDocumentTagDTO dto);
        Task<bool> Delete(long id);
    }
}
