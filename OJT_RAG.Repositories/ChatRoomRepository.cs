using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using System;

namespace OJT_RAG.Repositories
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        private readonly OJTRAGContext _db;

        public ChatRoomRepository(OJTRAGContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ChatRoom>> GetAllAsync()
        {
            return await _db.ChatRooms.ToListAsync();
        }

        public async Task<ChatRoom?> GetByIdAsync(long id)
        {
            return await _db.ChatRooms.FirstOrDefaultAsync(x => x.ChatRoomId == id);
        }

        public async Task<IEnumerable<ChatRoom>> GetByUserIdAsync(long userId)
        {
            return await _db.ChatRooms.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<ChatRoom> AddAsync(ChatRoom entity)
        {
            await _db.ChatRooms.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<ChatRoom> UpdateAsync(ChatRoom entity)
        {
            _db.ChatRooms.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;

            _db.ChatRooms.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
