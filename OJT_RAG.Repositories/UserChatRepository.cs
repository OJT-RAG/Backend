using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;

namespace OJT_RAG.Repositories
{
    public class UserChatRepository : IUserChatRepository
    {
        private readonly OJTRAGContext _db;

        public UserChatRepository(OJTRAGContext db)
        {
            _db = db;
        }

        public async Task<UserChatMessage> AddAsync(UserChatMessage message)
        {
            await _db.UserChatMessages.AddAsync(message);
            await _db.SaveChangesAsync();
            return message;
        }

        public async Task<IEnumerable<UserChatMessage>> GetConversation(long user1, long user2)
        {
            return await _db.UserChatMessages
                .Where(x =>
                    (x.SenderId == user1 && x.ReceiverId == user2) ||
                    (x.SenderId == user2 && x.ReceiverId == user1))
                .OrderBy(x => x.SentAt)
                .ToListAsync();
        }
    }
}
