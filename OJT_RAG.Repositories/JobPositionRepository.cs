using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;

namespace OJT_RAG.Repositories
{
    public class JobPositionRepository : IJobPositionRepository
    {
        private readonly OJTRAGContext _context;

        public JobPositionRepository(OJTRAGContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobPosition>> GetAllAsync()
            => await _context.JobPositions.ToListAsync();

        public async Task<JobPosition?> GetByIdAsync(long id)
            => await _context.JobPositions.FindAsync(id);

        public async Task<JobPosition> AddAsync(JobPosition entity)
        {
            await _context.JobPositions.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<JobPosition> UpdateAsync(JobPosition entity)
        {
            _context.JobPositions.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var item = await _context.JobPositions.FindAsync(id);
            if (item == null) return false;
            _context.JobPositions.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
