using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface ISemesterCompanyRepository
    {
        Task<IEnumerable<SemesterCompany>> GetAllAsync();
        Task<SemesterCompany?> GetByIdAsync(long id);
        Task<IEnumerable<SemesterCompany>> GetBySemesterIdAsync(long semesterId);
        Task<IEnumerable<SemesterCompany>> GetByCompanyIdAsync(long companyId);

        Task<bool> ExistsAsync(long semesterId, long companyId);

        Task AddAsync(SemesterCompany entity);
        Task UpdateAsync(SemesterCompany entity);
        Task<bool> DeleteAsync(long id);
    }
}
