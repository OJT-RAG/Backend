using System;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Enums;
using OJT_RAG.Repositories.Interfaces;

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
        public async Task<bool> UpdateAccountStatusAsync(
     long userId,
     AccountStatusEnum status)
        {
            const string sql = """
        UPDATE "User"
        SET account_status = @status::account_status_enum,
            update_at = @updateAt
        WHERE user_id = @userId
    """;

            var statusParam = new NpgsqlParameter("status", status.ToString()); // 🔥 STRING
            var userIdParam = new NpgsqlParameter("userId", userId);
            var updateAtParam = new NpgsqlParameter("updateAt", DateTime.UtcNow);

            var rows = await _db.Database.ExecuteSqlRawAsync(
                sql,
                statusParam,
                updateAtParam,
                userIdParam
            );

            return rows > 0;
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
