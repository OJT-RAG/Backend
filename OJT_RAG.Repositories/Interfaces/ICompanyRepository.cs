using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task<long> GetNextId();

        Task<IEnumerable<Company>> GetAll();
        Task<Company?> GetById(long id);
        Task Add(Company entity);
        Task Update(Company entity);
        Task Delete(long id);
    }
}
