using Microsoft.AspNetCore.SignalR;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Hubs;

namespace OJT_RAG.Services
{
    public class UserChatService
    {
        private readonly IUserChatRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IHubContext<UserChatHub> _hub;

        public UserChatService(
            IUserChatRepository repo,
            IUserRepository userRepo,
            IHubContext<UserChatHub> hub)
        {
            _repo = repo;
            _userRepo = userRepo;
            _hub = hub;
        }

        public async Task<UserChatMessage> SendMessage(long senderId, long receiverId, string content)
        {
            if (!await _userRepo.ExistsAsync(senderId))
                throw new Exception("Sender không tồn tại");

            if (!await _userRepo.ExistsAsync(receiverId))
                throw new Exception("Receiver không tồn tại");

            var msg = new UserChatMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.UtcNow.ToLocalTime()
            };

            var saved = await _repo.AddAsync(msg);
            Console.WriteLine($"[DB] Saving message: {senderId} -> {receiverId}: {content}");
            // Push realtime
            await _hub.Clients.User(receiverId.ToString())
                .SendAsync("ReceiveMessage", saved);
            Console.WriteLine($"[SignalR] Pushed realtime to user {receiverId}");
            return saved;
        }

        public async Task<List<UserChatMessage>> GetConversation(long user1, long user2)
        {
            return await _repo.GetConversation(user1, user2);
        }
    }
}
