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

        public Task<IEnumerable<Semester>> GetAllAsync() => _repo.GetAllAsync();

        public Task<Semester?> GetByIdAsync(long id) => _repo.GetByIdAsync(id);

        public async Task<Semester> CreateAsync(Semester request)
        {
           
            return await _repo.AddAsync(request);
        }

        public async Task<Semester?> UpdateAsync(long id, Semester request)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Name = request.Name;
            existing.StartDate = request.StartDate;
            existing.EndDate = request.EndDate;
            existing.IsActive = request.IsActive;
            

            return await _repo.UpdateAsync(existing);
        }

        public Task<bool> DeleteAsync(long id) => _repo.DeleteAsync(id);
    }
}
