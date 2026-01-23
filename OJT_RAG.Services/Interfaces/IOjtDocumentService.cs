using OJT_RAG.DTOs.OjtDocumentDTO;
using OJT_RAG.ModelView.OjtDocumentModelView;
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Services.Interfaces
{
    public interface IOjtDocumentService
    {
        Task<IEnumerable<OjtDocumentModelView>> GetAllAsync();
        Task<OjtDocumentModelView?> GetByIdAsync(long id);
        Task<IEnumerable<OjtDocumentModelView>> GetByTagTypeAsync(string type);
        Task<OjtDocumentModelView> CreateAsync(CreateOjtDocumentDTO dto);
        Task<OjtDocumentModelView> UpdateAsync(UpdateOjtDocumentDTO dto);
        Task<bool> DeleteAsync(long id);
        Task<(byte[] fileBytes, string fileName, string contentType)?> DownloadAsync(long id);
        Task<IEnumerable<OjtDocumentModelView>> GetBySemesterAsync(long semesterId);
        Task<IEnumerable<Documenttag>> GetTags(long ojtDocumentId);
        Task AddTag(long documentId, long tagId);
        Task RemoveTag(long documentId, long tagId);

    }
}
