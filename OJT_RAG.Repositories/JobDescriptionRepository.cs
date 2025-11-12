
using OJT_RAG.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Context;

namespace OJT_RAG.Repositories
{
    public class JobDescriptionRepository : IJobDescriptionRepository
    {
        private readonly OJTRAGContext _context;

        public JobDescriptionRepository(OJTRAGContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobDescription>> GetAll()
        {
            return await _context.JobDescriptions.ToListAsync();
        }

        public async Task<JobDescription?> GetById(int id)
        {
            return await _context.JobDescriptions.FindAsync(id);
        }

        public async Task<JobDescription> Add(JobDescription entity)
        {
            _context.JobDescriptions.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<JobDescription> Update(JobDescription entity)
        {
            _context.JobDescriptions.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _context.JobDescriptions.FindAsync(id);
            if (entity == null) return false;

            _context.JobDescriptions.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
