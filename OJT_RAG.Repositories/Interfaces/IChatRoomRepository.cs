using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IChatRoomRepository
    {
        Task<IEnumerable<ChatRoom>> GetAllAsync();
        Task<ChatRoom?> GetByIdAsync(long id);
        Task<IEnumerable<ChatRoom>> GetByUserIdAsync(long userId);
        Task<ChatRoom> AddAsync(ChatRoom entity);
        Task<ChatRoom> UpdateAsync(ChatRoom entity);
        Task<bool> DeleteAsync(long id);
    }
}
