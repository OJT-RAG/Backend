using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface ICompanyDocumentTagRepository
    {
        Task<IEnumerable<Companydocumenttag>> GetAllAsync();
        Task<Companydocumenttag?> GetByIdAsync(long id);
        Task AddAsync(Companydocumenttag entity);
        Task UpdateAsync(Companydocumenttag entity);
        Task<bool> DeleteAsync(long id);
    }
}
