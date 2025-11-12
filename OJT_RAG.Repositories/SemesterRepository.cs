using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;

namespace OJT_RAG.Repositories
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly OJTRAGContext _context;

        public SemesterRepository(OJTRAGContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Semester>> GetAllAsync()
            => await _context.Semesters.ToListAsync();

        public async Task<Semester?> GetByIdAsync(long id)
            => await _context.Semesters.FindAsync(id);

        public async Task<Semester> AddAsync(Semester entity)
        {
            await _context.Semesters.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Semester> UpdateAsync(Semester entity)
        {
            _context.Semesters.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var item = await _context.Semesters.FindAsync(id);
            if (item == null) return false;
            _context.Semesters.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
