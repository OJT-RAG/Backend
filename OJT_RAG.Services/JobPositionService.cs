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

        public Task<IEnumerable<JobPosition>> GetAllAsync() => _repo.GetAllAsync();

        public Task<JobPosition?> GetByIdAsync(long id) => _repo.GetByIdAsync(id);

        public async Task<JobPosition> CreateAsync(JobPosition request)
        {
            request.CreateAt = DateTime.UtcNow;
            return await _repo.AddAsync(request);
        }

        public async Task<JobPosition?> UpdateAsync(long id, JobPosition request)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            // Cập nhật các trường thông dụng (thay đổi theo entity thực tế của bạn)
            existing.JobTitle = request.JobTitle;
            existing.Requirements = request.Requirements;
            existing.Benefit = request.Benefit;
            existing.Location = request.Location;
            existing.SalaryRange = request.SalaryRange;
            existing.IsActive = request.IsActive;
            existing.MajorId = request.MajorId;
            existing.SemesterId = request.SemesterId;
            existing.UpdateAt = DateTime.UtcNow;

            return await _repo.UpdateAsync(existing);
        }

        public Task<bool> DeleteAsync(long id) => _repo.DeleteAsync(id);
    }
}
