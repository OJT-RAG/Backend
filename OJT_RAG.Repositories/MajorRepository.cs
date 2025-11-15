using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;

namespace OJT_RAG.Repositories.Repositories
{
    public class MajorRepository : IMajorRepository
    {
        private readonly OJTRAGContext _context;

        public MajorRepository(OJTRAGContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Major>> GetAll()
            => await _context.Majors.ToListAsync();

        public async Task<Major?> GetById(long id)
            => await _context.Majors.FindAsync(id);

        public async Task Add(Major entity)
        {
            await _context.Majors.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Major entity)
        {
            _context.Majors.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var existing = await _context.Majors.FindAsync(id);
            if (existing != null)
            {
                _context.Majors.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<long> GetNextId()
        {
            return await _context.Majors.MaxAsync(m => (long?)m.MajorId) + 1 ?? 1;
        }
    }
}
