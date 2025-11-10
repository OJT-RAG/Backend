using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services
{
    public class MajorService : IMajorService
    {
        private readonly IMajorRepository _repo;

        public MajorService(IMajorRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Major>> GetAllMajors() => await _repo.GetAllAsync();

        public async Task<Major?> GetMajor(long id) => await _repo.GetByIdAsync(id);

        public async Task<Major> CreateMajor(Major major)
        {
            major.CreateAt = DateTime.UtcNow;
            return await _repo.AddAsync(major);
        }

        public async Task<Major?> UpdateMajor(long id, Major major)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            existing.MajorTitle = major.MajorTitle;
            existing.MajorCode = major.MajorCode;
            existing.Description = major.Description;
            existing.UpdateAt = DateTime.UtcNow;

            return await _repo.UpdateAsync(existing);
        }

        public async Task<bool> DeleteMajor(long id) => await _repo.DeleteAsync(id);
    }
}
