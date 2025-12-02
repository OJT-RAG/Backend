using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface ICompanyDocumentRepository
    {
        Task<IEnumerable<Companydocument>> GetAllAsync();
        Task<Companydocument?> GetByIdAsync(long id);
        Task<IEnumerable<Companydocument>> GetBySemesterCompanyIdAsync(long semCompanyId);
        Task AddAsync(Companydocument entity);
        Task UpdateAsync(Companydocument entity);
        Task<bool> DeleteAsync(long id);
    }
}
