using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IOjtDocumentTagRepository
    {
        Task<bool> ExistsAsync(long ojtDocumentId, long documentTagId);
        Task<IEnumerable<Documenttag>> GetTagsByDocumentId(long ojtDocumentId);
        Task AddAsync(Ojtdocumenttag entity);
        Task RemoveAsync(long ojtDocumentId, long documentTagId);

    }
}
