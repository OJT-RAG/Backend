using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetAllAsync();
        Task<Message?> GetByIdAsync(long id);
        Task<IEnumerable<Message>> GetByChatRoomIdAsync(long chatRoomId);
        Task<Message> AddAsync(Message entity);
        Task<Message> UpdateAsync(Message entity);
        Task<bool> DeleteAsync(long id);
    }
}
