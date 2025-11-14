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
            var item = await _context.Majors.FindAsync(id);
            if (item != null)
            {
                _context.Majors.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<long> GetNextId()
        {
            var max = await _context.Majors.MaxAsync(x => (long?)x.MajorId) ?? 0;
            return max + 1;
        }
    }
}
