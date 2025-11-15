using OJT_RAG.DTOs.JobPositionDTO;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services
{
    public class JobPositionService : IJobPositionService
    {
        private readonly IJobPositionRepository _repo;

        public JobPositionService(IJobPositionRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<JobPosition>> GetAllAsync()
            => await _repo.GetAll();

        public async Task<JobPosition?> GetByIdAsync(long id)
            => await _repo.GetById(id);

        public async Task<JobPosition> CreateAsync(JobPositionCreateDTO dto)
        {
            var newId = await _repo.GetNextId();

            var entity = new JobPosition
            {
                JobPositionId = newId,
                MajorId = dto.MajorId,
                SemesterId = dto.SemesterId,
                JobTitle = dto.JobTitle,
                Requirements = dto.Requirements,
                Benefit = dto.Benefit,
                Location = dto.Location,
                SalaryRange = dto.SalaryRange,
                IsActive = true,
                CreateAt = DateTime.UtcNow.ToLocalTime(),
                UpdateAt = DateTime.UtcNow.ToLocalTime()
            };

            await _repo.Add(entity);
            return entity;
        }

        public async Task<JobPosition?> UpdateAsync(long id, JobPositionUpdateDTO dto)
        {
            var existing = await _repo.GetById(id);
            if (existing == null) return null;

            existing.MajorId = dto.MajorId ?? existing.MajorId;
            existing.SemesterId = dto.SemesterId ?? existing.SemesterId;
            existing.JobTitle = dto.JobTitle ?? existing.JobTitle;
            existing.Requirements = dto.Requirements ?? existing.Requirements;
            existing.Benefit = dto.Benefit ?? existing.Benefit;
            existing.Location = dto.Location ?? existing.Location;
            existing.SalaryRange = dto.SalaryRange ?? existing.SalaryRange;
            existing.IsActive = dto.IsActive ?? existing.IsActive;
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
