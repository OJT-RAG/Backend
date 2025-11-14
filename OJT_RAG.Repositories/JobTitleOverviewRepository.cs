using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;

namespace OJT_RAG.Repositories
{
    public class JobTitleOverviewRepository : IJobTitleOverviewRepository
    {
        private readonly OJTRAGContext _context;

        public JobTitleOverviewRepository(OJTRAGContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobTitleOverview>> GetAll()
        {
            return await _context.JobTitleOverviews.ToListAsync();
        }

        public async Task<JobTitleOverview?> GetById(long id)
        {
            return await _context.JobTitleOverviews.FindAsync(id);
        }

        public async Task<JobTitleOverview> Add(JobTitleOverview model)
        {
            _context.JobTitleOverviews.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<JobTitleOverview> Update(JobTitleOverview model)
        {
            _context.JobTitleOverviews.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<bool> Delete(int id)
        {
            var data = await _context.JobTitleOverviews.FindAsync(id);
            if (data == null) return false;

            _context.JobTitleOverviews.Remove(data);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
