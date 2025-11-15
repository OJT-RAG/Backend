using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.DTOs.Major;
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

        public Task<IEnumerable<Major>> GetAllAsync()
            => _repo.GetAll();

        public Task<Major?> GetByIdAsync(long id)
            => _repo.GetById(id);

        public async Task<Major> CreateAsync(CreateMajorDTO dto)
        {
            var newId = await _repo.GetNextId();

            var major = new Major
            {
                MajorId = newId,
                MajorTitle = dto.MajorTitle,
                MajorCode = dto.MajorCode,
                Description = dto.Description,
                CreateAt = DateTime.UtcNow.ToLocalTime(),
                UpdateAt = DateTime.UtcNow.ToLocalTime()
            };

            await _repo.Add(major);
            return major;
        }

        public async Task<Major?> UpdateAsync(UpdateMajorDTO dto)
        {
            var existing = await _repo.GetById(dto.MajorId);
            if (existing == null) return null;

            existing.MajorTitle = dto.MajorTitle ?? existing.MajorTitle;
            existing.MajorCode = dto.MajorCode ?? existing.MajorCode;
            existing.Description = dto.Description ?? existing.Description;
            existing.UpdateAt = DateTime.UtcNow.ToLocalTime();

            await _repo.Update(existing);
            return existing;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var existing = await _repo.GetById(id);
            if (existing == null) return false;

            await _repo.Delete(id);
            return true;
        }
    }
}
