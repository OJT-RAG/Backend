using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IDocumentTagRepository
    {
        Task<IEnumerable<Documenttag>> GetAllAsync();
        Task<Documenttag?> GetByIdAsync(long id);
        Task<Documenttag> AddAsync(Documenttag entity);
        Task<Documenttag> UpdateAsync(Documenttag entity);
        Task<bool> DeleteAsync(long id);
    }
}
