using OJT_RAG.DTOs.OjtDocumentTag;
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Services.Interfaces
{
    public interface IOjtDocumentTagService
    {
        Task<IEnumerable<Ojtdocumenttag>> GetAll();
        Task<Ojtdocumenttag?> GetById(long id);
        Task<bool> Create(CreateOjtDocumentTagDTO dto);
        Task<bool> Update(UpdateOjtDocumentTagDTO dto);
        Task<bool> Delete(long id);
    }
}
