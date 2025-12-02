using OJT_RAG.DTOs.JobPositionDTO;
using OJT_RAG.ModelView.JobPositionModelView;
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

        public async Task<IEnumerable<JobPositionModelView>> GetAll()
        {
            return (await _repo.GetAllAsync()).Select(x => new JobPositionModelView
            {
                JobPositionId = x.JobPositionId,
                MajorId = x.MajorId,
                SemesterId = x.SemesterId,
                JobTitle = x.JobTitle,
                Requirements = x.Requirements,
                Benefit = x.Benefit,
                Location = x.Location,
                SalaryRange = x.SalaryRange,
                IsActive = x.IsActive,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            });
        }

        public async Task<JobPositionModelView?> GetById(long id)
        {
            var x = await _repo.GetByIdAsync(id);
            if (x == null) return null;

            return new JobPositionModelView
            {
                JobPositionId = x.JobPositionId,
                MajorId = x.MajorId,
                SemesterId = x.SemesterId,
                JobTitle = x.JobTitle,
                Requirements = x.Requirements,
                Benefit = x.Benefit,
                Location = x.Location,
                SalaryRange = x.SalaryRange,
                IsActive = x.IsActive,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            };
        }

        public async Task<bool> Create(CreateJobPositionDTO dto)
        {
            var entity = new JobPosition
            {
                MajorId = dto.MajorId,
                SemesterId = dto.SemesterId,
                JobTitle = dto.JobTitle,
                Requirements = dto.Requirements,
                Benefit = dto.Benefit,
                Location = dto.Location,
                SalaryRange = dto.SalaryRange,
                IsActive = dto.IsActive,
                CreateAt = DateTime.UtcNow.ToLocalTime(),
                UpdateAt = DateTime.UtcNow.ToLocalTime()
            };

            await _repo.AddAsync(entity);
            return true;
        }

        public async Task<bool> Update(UpdateJobPositionDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.JobPositionId);
            if (entity == null) return false;

            entity.MajorId = dto.MajorId;
            entity.SemesterId = dto.SemesterId;
            entity.JobTitle = dto.JobTitle;
            entity.Requirements = dto.Requirements;
            entity.Benefit = dto.Benefit;
            entity.Location = dto.Location;
            entity.SalaryRange = dto.SalaryRange;
            entity.IsActive = dto.IsActive;
            entity.UpdateAt = DateTime.UtcNow.ToLocalTime();

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> Delete(long id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
