using OJT_RAG.Repositories.Entities;
using OJT_RAG.Services.DTOs.Major;

namespace OJT_RAG.Services.Interfaces
{
    public interface IMajorService
    {
        Task<IEnumerable<Major>> GetAllAsync();
        Task<Major?> GetByIdAsync(long id);
        Task<Major> CreateAsync(CreateMajorDTO dto);
        Task<Major?> UpdateAsync(UpdateMajorDTO dto);
        Task<bool> DeleteAsync(long id);
    }
}
