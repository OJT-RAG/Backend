using OJT_RAG.DTOs.DocumentTagDTO;
using OJT_RAG.ModelView.DocumentTagModelView;

namespace OJT_RAG.Services.Interfaces
{
    public interface IDocumentTagService
    {
        Task<IEnumerable<DocumentTagModelView>> GetAll();
        Task<DocumentTagModelView?> GetById(long id);
        Task<bool> Create(CreateDocumentTagDTO dto);
        Task<bool> Update(UpdateDocumentTagDTO dto);
        Task<bool> Delete(long id);
    }
}
