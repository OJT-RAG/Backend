using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(long id);
        Task<User> AddAsync(User entity);
        Task<User> UpdateAsync(User entity);
        Task<bool> DeleteAsync(long id);
    }
}
