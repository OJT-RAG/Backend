using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IOjtDocumentTagRepository
    {
        Task AddAsync(Ojtdocumenttag entity);
        Task RemoveAsync(long documentId, long tagId);
        Task<bool> ExistsAsync(long documentId, long tagId);
        Task<IEnumerable<Documenttag>> GetTagsByDocumentId(long documentId);
    }

}
