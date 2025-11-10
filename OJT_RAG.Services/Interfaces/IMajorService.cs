using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Services.Interfaces
{
    public interface IMajorService
    {
        Task<IEnumerable<Major>> GetAllMajors();
        Task<Major?> GetMajor(long id);
        Task<Major> CreateMajor(Major major);
        Task<Major?> UpdateMajor(long id, Major major);
        Task<bool> DeleteMajor(long id);
    }
}
