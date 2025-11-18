using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface ICompanyDocumentRepository
    {
        Task<IEnumerable<Companydocument>> GetAllAsync();
        Task<Companydocument?> GetByIdAsync(long id);
        Task<IEnumerable<Companydocument>> GetBySemesterCompanyIdAsync(long semId);
        Task<Companydocument> AddAsync(Companydocument doc);
        Task<Companydocument> UpdateAsync(Companydocument doc);
        Task<bool> DeleteAsync(long id);
    }
}
