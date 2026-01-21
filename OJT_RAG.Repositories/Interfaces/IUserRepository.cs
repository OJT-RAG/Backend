using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Enums;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(long id);
        Task<User?> GetByEmailAsync(string email);
        Task<User> AddAsync(User entity);
        Task<User> UpdateAsync(User entity);
        Task<bool> UpdateAccountStatusAsync(long userId, AccountStatusEnum status);
        Task<bool> DeleteAsync(long id);
        Task<bool> ExistsAsync(long userId);
    }
}
