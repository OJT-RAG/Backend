using OJT_RAG.DTOs.SemesterDTO;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services
{
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository _repo;

        public SemesterService(ISemesterRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Semester>> GetAllAsync()
            => await _repo.GetAll();

        public async Task<Semester?> GetByIdAsync(long id)
            => await _repo.GetById(id);

        public async Task<Semester> CreateAsync(SemesterCreateDTO dto)
        {
            var newId = await _repo.GetNextId();

            var entity = new Semester
            {
                SemesterId = newId,
                Name = dto.Name,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = dto.IsActive ?? true,
            };

            await _repo.Add(entity);
            return entity;
        }

        public async Task<Semester?> UpdateAsync(long id, SemesterUpdateDTO dto)
        {
            var existing = await _repo.GetById(id);
            if (existing == null) return null;

            existing.Name = dto.Name ?? existing.Name;
            existing.StartDate = dto.StartDate ?? existing.StartDate;
            existing.EndDate = dto.EndDate ?? existing.EndDate;
            existing.IsActive = dto.IsActive ?? existing.IsActive;

            await _repo.Update(existing);
            return existing;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _repo.GetById(id);
            if (entity == null) return false;

            await _repo.Delete(id);
            return true;
        }
    }
}
