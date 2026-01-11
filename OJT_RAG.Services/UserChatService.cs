using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;

namespace OJT_RAG.Services
{
    public class UserChatService
    {
        private readonly IUserChatRepository _repo;

        public UserChatService(IUserChatRepository repo)
        {
            _repo = repo;
        }

        public async Task<UserChatMessage> SendMessage(long senderId, long receiverId, string content)
        {
            var msg = new UserChatMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.UtcNow
            };

            return await _repo.AddAsync(msg);
        }

        public async Task<List<UserChatMessage>> GetConversation(long user1, long user2)
        {
            return await _repo.GetConversation(user1, user2);
        }
    }
}
