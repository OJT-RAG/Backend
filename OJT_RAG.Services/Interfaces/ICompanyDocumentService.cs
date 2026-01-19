using OJT_RAG.DTOs.CompanyDocumentDTO;
using OJT_RAG.ModelViews.CompanyDocument;
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Services.Interfaces
{
    public interface ICompanyDocumentService
    {
        Task<IEnumerable<CompanyDocumentModelView>> GetAll();
        Task<CompanyDocumentModelView?> GetById(long id);
        Task<IEnumerable<CompanyDocumentModelView>> GetBySemester(long semesterCompanyId);
        Task<bool> Create(CreateCompanyDocumentDTO dto);
        Task<bool> Update(UpdateCompanyDocumentDTO dto);
        Task<bool> Delete(long id);
        Task<(byte[] fileBytes, string fileName, string contentType)?> Download(long id);
        Task<IEnumerable<Documenttag>> GetTags(long companyDocumentId);
        Task AddTag(long documentId, long tagId);
        Task RemoveTag(long documentId, long tagId);

    }
}
