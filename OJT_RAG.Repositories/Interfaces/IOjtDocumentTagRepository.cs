using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IOjtDocumentTagRepository
    {
        Task<IEnumerable<Ojtdocumenttag>> GetAllAsync();
        Task<Ojtdocumenttag?> GetByIdAsync(long id);
        Task AddAsync(Ojtdocumenttag entity);
        Task UpdateAsync(Ojtdocumenttag entity);
        Task<bool> DeleteAsync(long id);
    }
}
