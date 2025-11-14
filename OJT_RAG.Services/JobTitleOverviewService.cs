
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

        public Task<IEnumerable<JobTitleOverview>> GetAll() => _repo.GetAll();
        public Task<JobTitleOverview?> GetById(int id) => _repo.GetById(id);
        public Task<JobTitleOverview> Create(JobTitleOverview model) => _repo.Add(model);
        public Task<JobTitleOverview> Update(JobTitleOverview model) => _repo.Update(model);
        public Task<bool> Delete(int id) => _repo.Delete(id);
    }
}
