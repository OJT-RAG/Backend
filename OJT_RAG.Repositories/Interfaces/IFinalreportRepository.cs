using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IFinalreportRepository
    {
        Task<IEnumerable<Finalreport>> GetAllAsync();
        Task<Finalreport?> GetByIdAsync(long id);
        Task<IEnumerable<Finalreport>> GetByUserIdAsync(long userId);
        Task<IEnumerable<Finalreport>> GetBySemesterIdAsync(long semesterId);
        Task<IEnumerable<Finalreport>> GetByJobPositionIdAsync(long jobPositionId);

        Task<Finalreport> AddAsync(Finalreport entity);
        Task<Finalreport> UpdateAsync(Finalreport entity);
        Task<bool> DeleteAsync(long id);
    }
}
