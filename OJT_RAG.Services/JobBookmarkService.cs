using OJT_RAG.DTOs.JobBookmarkDTO;
using OJT_RAG.ModelView.JobBookmarkModelView;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services
{
    public class JobBookmarkService : IJobBookmarkService
    {
        private readonly IJobBookmarkRepository _repo;

        public JobBookmarkService(IJobBookmarkRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<JobBookmarkModelView>> GetAll()
        {
            return (await _repo.GetAllAsync()).Select(x => new JobBookmarkModelView
            {
                JobBookmarkId = x.JobBookmarkId,
                UserId = x.UserId,
                JobPositionId = x.JobPositionId,
                CreateAt = x.CreateAt
            });
        }

        public async Task<JobBookmarkModelView?> GetById(long id)
        {
            var x = await _repo.GetByIdAsync(id);
            if (x == null) return null;

            return new JobBookmarkModelView
            {
                JobBookmarkId = x.JobBookmarkId,
                UserId = x.UserId,
                JobPositionId = x.JobPositionId,
                CreateAt = x.CreateAt
            };
        }

        public async Task<IEnumerable<JobBookmarkModelView>> GetByUserId(long userId)
        {
            return (await _repo.GetByUserIdAsync(userId)).Select(x => new JobBookmarkModelView
            {
                JobBookmarkId = x.JobBookmarkId,
                UserId = x.UserId,
                JobPositionId = x.JobPositionId,
                CreateAt = x.CreateAt
            });
        }

        public async Task<bool> Create(CreateJobBookmarkDTO dto)
        {
            var entity = new JobBookmark
            {
                UserId = dto.UserId,
                JobPositionId = dto.JobPositionId,
                CreateAt = DateTime.UtcNow.ToLocalTime()
            };

            await _repo.AddAsync(entity);
            return true;
        }

        public async Task<bool> Update(UpdateJobBookmarkDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.JobBookmarkId);
            if (entity == null) return false;

            if (dto.UserId != null) entity.UserId = dto.UserId;
            if (dto.JobPositionId != null) entity.JobPositionId = dto.JobPositionId;

            entity.CreateAt = entity.CreateAt ?? DateTime.UtcNow.ToLocalTime();

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> Delete(long id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
