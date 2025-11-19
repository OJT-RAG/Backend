using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface ISemesterCompanyRepository
    {
        Task<IEnumerable<SemesterCompany>> GetAllAsync();
        Task<SemesterCompany?> GetByIdAsync(long id);
        Task<IEnumerable<SemesterCompany>> GetBySemesterIdAsync(long semesterId);
        Task<IEnumerable<SemesterCompany>> GetByCompanyIdAsync(long companyId);

        Task<SemesterCompany> AddAsync(SemesterCompany entity);
        Task<SemesterCompany> UpdateAsync(SemesterCompany entity);
        Task<bool> DeleteAsync(long id);
    }
}
