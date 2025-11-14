using OJT_RAG.ModelViews.Major;
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Services.Interfaces
{
    public interface IMajorService
    {
        Task<IEnumerable<Major>> GetAllAsync();
        Task<Major?> GetByIdAsync(long id);
        Task<Major> CreateAsync(MajorCreateModel dto);
        Task<Major?> UpdateAsync(MajorUpdateModel dto);
        Task<bool> DeleteAsync(long id);
    }
}
