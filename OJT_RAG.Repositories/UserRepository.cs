using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;

namespace OJT_RAG.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OJTRAGContext _context;

        public UserRepository(OJTRAGContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
            => await _context.Users.ToListAsync();

        public async Task<User?> GetByIdAsync(long id)
            => await _context.Users.FindAsync(id);

        public async Task<User> AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var item = await _context.Users.FindAsync(id);
            if (item == null) return false;
            _context.Users.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
