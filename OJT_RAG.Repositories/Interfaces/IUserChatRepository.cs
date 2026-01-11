using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IUserChatRepository
    {
        Task<UserChatMessage> AddAsync(UserChatMessage message);
        Task<List<UserChatMessage>> GetConversation(long user1, long user2);
    }
}
