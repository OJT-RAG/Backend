using OJT_RAG.DTOs.OjtDocumentDTO;
using OJT_RAG.ModelView.OjtDocumentModelView;

namespace OJT_RAG.Services.Interfaces
{
    public interface IOjtDocumentService
    {
        Task<IEnumerable<OjtDocumentModelView>> GetAllAsync();
        Task<OjtDocumentModelView?> GetByIdAsync(long id);
        Task<OjtDocumentModelView> CreateAsync(CreateOjtDocumentDTO dto);
        Task<OjtDocumentModelView> UpdateAsync(UpdateOjtDocumentDTO dto);
        Task<bool> DeleteAsync(long id);
    }
}
