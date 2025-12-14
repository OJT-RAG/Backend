using Microsoft.AspNetCore.SignalR;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.API.Hubs
{
    public class UserChatHub : Hub
    {
        private readonly OJTRAGContext _db;

        public UserChatHub(OJTRAGContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gửi tin nhắn giữa 2 user và lưu DB
        /// </summary>
        public async Task SendMessage(
            long senderId,
            long receiverId,
            string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new HubException("Message content is empty");

            // 1️⃣ LƯU DB
            var message = new UserChatMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.UtcNow
            };

            _db.UserChatMessages.Add(message);
            await _db.SaveChangesAsync();

            // 2️⃣ GỬI REALTIME QUA GROUP (ĐÚNG)
            await Clients.Groups(
                senderId.ToString(),
                receiverId.ToString()
            ).SendAsync("ReceiveMessage", new
            {
                message.Id,
                message.SenderId,
                message.ReceiverId,
                message.Content,
                message.SentAt
            });
        }

        /// <summary>
        /// Khi user connect → join group = userId
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"];

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    userId
                );
            }

            await base.OnConnectedAsync();
        }
    }
}
