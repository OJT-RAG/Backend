using OJT_RAG.ModelViews.Major;
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

        public async Task<IEnumerable<Major>> GetAllAsync()
            => await _repo.GetAll();

        public async Task<Major?> GetByIdAsync(long id)
            => await _repo.GetById(id);

        public async Task<Major> CreateAsync(MajorCreateModel dto)
        {
            var newId = await _repo.GetNextId();

            var major = new Major
            {
                MajorId = newId,
                MajorTitle = dto.Major_Name,
                Description = dto.Description,
                CreateAt = DateTime.UtcNow.ToLocalTime(),
                UpdateAt = DateTime.UtcNow.ToLocalTime()
            };

            await _repo.Add(major);
            return major;
        }

        public async Task<Major?> UpdateAsync(MajorUpdateModel dto)
        {
            var existing = await _repo.GetById(dto.Major_ID);
            if (existing == null)
                return null;

            existing.MajorTitle = dto.Major_Name ?? existing.MajorTitle;
            existing.Description = dto.Description ?? existing.Description;
            existing.UpdateAt = DateTime.UtcNow.ToLocalTime();

            await _repo.Update(existing);
            return existing;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var existing = await _repo.GetById(id);
            if (existing == null)
                return false;

            await _repo.Delete(id);
            return true;
        }
    }
}
