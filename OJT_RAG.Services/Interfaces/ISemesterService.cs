using OJT_RAG.DTOs.SemesterDTO;
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Services.Interfaces
{
    public interface ISemesterService
    {
        Task<IEnumerable<Semester>> GetAllAsync();
        Task<Semester?> GetByIdAsync(long id);
        Task<Semester> CreateAsync(SemesterCreateDTO dto);
        Task<Semester?> UpdateAsync(long id, SemesterUpdateDTO dto);
        Task<bool> DeleteAsync(long id);
    }
}
