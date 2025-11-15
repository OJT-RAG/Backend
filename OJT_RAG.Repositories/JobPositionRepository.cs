using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;

using System;
using OJT_RAG.Repositories.Context;

namespace OJT_RAG.Repositories
{
    public class JobPositionRepository : IJobPositionRepository
    {
        private readonly OJTRAGContext _context;

        public JobPositionRepository(OJTRAGContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobPosition>> GetAll()
            => await _context.JobPositions.ToListAsync();

        public async Task<JobPosition?> GetById(long id)
            => await _context.JobPositions.FindAsync(id);

        public async Task<long> GetNextId()
        {
            var maxId = await _context.JobPositions.MaxAsync(p => (long?)p.JobPositionId) ?? 0;
            return maxId + 1;
        }

        public async Task Add(JobPosition entity)
        {
            _context.JobPositions.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(JobPosition entity)
        {
            _context.JobPositions.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var entity = await GetById(id);
            if (entity != null)
            {
                _context.JobPositions.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
