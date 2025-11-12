using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(long id);
        Task<User> CreateAsync(User request);
        Task<User?> UpdateAsync(long id, User request);
        Task<bool> DeleteAsync(long id);
    }
}
