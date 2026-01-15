using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using System;

namespace OJT_RAG.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OJTRAGContext _db;

        public UserRepository(OJTRAGContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(long id)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> AddAsync(User entity)
        {
            await _db.Users.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            _db.Users.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var u = await GetByIdAsync(id);
            if (u == null) return false;

            _db.Users.Remove(u);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ExistsAsync(long userId)
        {
            return await _db.Users.AnyAsync(u => u.UserId == userId);
        }
    }
}
