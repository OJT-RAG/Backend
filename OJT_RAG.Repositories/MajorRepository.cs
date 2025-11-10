using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Repositories.Context;

namespace OJT_RAG.Repositories
{
    public class MajorRepository : IMajorRepository
    {
        private readonly OJTRAGContext _context;

        public MajorRepository(OJTRAGContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Major>> GetAllAsync()
        {
            return await _context.Majors.ToListAsync();
        }

        public async Task<Major?> GetByIdAsync(long id)
        {
            return await _context.Majors.FindAsync(id);
        }

        public async Task<Major> AddAsync(Major major)
        {
            _context.Majors.Add(major);
            await _context.SaveChangesAsync();
            return major;
        }

        public async Task<Major> UpdateAsync(Major major)
        {
            _context.Majors.Update(major);
            await _context.SaveChangesAsync();
            return major;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var major = await _context.Majors.FindAsync(id);
            if (major == null) return false;

            _context.Majors.Remove(major);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
