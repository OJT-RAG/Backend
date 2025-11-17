using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using System;

namespace OJT_RAG.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly OJTRAGContext _db;

        public MessageRepository(OJTRAGContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            return await _db.Messages.ToListAsync();
        }

        public async Task<Message?> GetByIdAsync(long id)
        {
            return await _db.Messages.FirstOrDefaultAsync(x => x.MessageId == id);
        }

        public async Task<IEnumerable<Message>> GetByChatRoomIdAsync(long chatRoomId)
        {
            return await _db.Messages
                .Where(x => x.ChatRoomId == chatRoomId)
                .ToListAsync();
        }

        public async Task<Message> AddAsync(Message entity)
        {
            await _db.Messages.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Message> UpdateAsync(Message entity)
        {
            _db.Messages.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var msg = await GetByIdAsync(id);
            if (msg == null) return false;

            _db.Messages.Remove(msg);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
