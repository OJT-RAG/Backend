using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Services
{
    public class JobDescriptionService : IJobDescriptionService
    {
        private readonly IJobDescriptionRepository _repo;

        public JobDescriptionService(IJobDescriptionRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<JobDescription>> GetAll() => _repo.GetAll();
        public Task<JobDescription?> GetById(int id) => _repo.GetById(id);
        public Task<JobDescription> Create(JobDescription model) => _repo.Add(model);
        public Task<JobDescription> Update(JobDescription model) => _repo.Update(model);
        public Task<bool> Delete(int id) => _repo.Delete(id);
    }
}
