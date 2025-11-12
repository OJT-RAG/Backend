using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<User>> GetAllAsync() => _repo.GetAllAsync();

        public Task<User?> GetByIdAsync(long id) => _repo.GetByIdAsync(id);

        public async Task<User> CreateAsync(User request)
        {
            // TODO: production => hash password, validate email unique, set defaults
            request.CreateAt = DateTime.UtcNow;
            return await _repo.AddAsync(request);
        }

        public async Task<User?> UpdateAsync(long id, User request)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            // Update các field an toàn (không update password trực tiếp ở đây nếu không hash)
            existing.Fullname = request.Fullname;
            existing.Email = request.Email;
            existing.Phone = request.Phone;
            existing.MajorId = request.MajorId;
            existing.CompanyId = request.CompanyId;
            existing.Dob = request.Dob;
            existing.AvatarUrl = request.AvatarUrl;
            existing.CvUrl = request.CvUrl;
            existing.UpdateAt = DateTime.UtcNow;

            return await _repo.UpdateAsync(existing);
        }

        public Task<bool> DeleteAsync(long id) => _repo.DeleteAsync(id);
    }
}
