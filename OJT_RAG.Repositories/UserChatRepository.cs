using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using System;

namespace OJT_RAG.Repositories
{
    public class UserChatRepository : IUserChatRepository
    {
        private readonly OJTRAGContext _context;

        public UserChatRepository(OJTRAGContext context)
        {
            _context = context;
        }

        public async Task<UserChatMessage> AddAsync(UserChatMessage message)
        {
            _context.UserChatMessages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<UserChatMessage>> GetConversation(long user1, long user2)
        {
            return await _context.UserChatMessages
                .Where(m =>
                    (m.SenderId == user1 && m.ReceiverId == user2) ||
                    (m.SenderId == user2 && m.ReceiverId == user1)
                )
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }
    }
}
