using OJT_RAG.DTOs.JobTitleOverviewDTO;
using OJT_RAG.ModelView.JobTitleOverviewModelView;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services
{
    public class JobTitleOverviewService : IJobTitleOverviewService
    {
        private readonly IJobTitleOverviewRepository _repo;

        public JobTitleOverviewService(IJobTitleOverviewRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<JobTitleOverviewModelView>> GetAll()
        {
            return (await _repo.GetAllAsync()).Select(x => new JobTitleOverviewModelView
            {
                JobTitleId = x.JobTitleId,
                JobTitle = x.JobTitle,
                PositionAmount = x.PositionAmount
            });
        }

        public async Task<JobTitleOverviewModelView?> GetById(long id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null) return null;

            return new JobTitleOverviewModelView
            {
                JobTitleId = data.JobTitleId,
                JobTitle = data.JobTitle,
                PositionAmount = data.PositionAmount
            };
        }

        public async Task<bool> Create(CreateJobTitleOverviewDTO dto)
        {
            var entity = new JobTitleOverview
            {
                JobTitle = dto.JobTitle,
                PositionAmount = dto.PositionAmount
            };

            await _repo.AddAsync(entity);
            return true;
        }

        public async Task<bool> Update(UpdateJobTitleOverviewDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.JobTitleId);
            if (entity == null) return false;

            entity.JobTitle = dto.JobTitle;
            entity.PositionAmount = dto.PositionAmount;

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> Delete(long id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
