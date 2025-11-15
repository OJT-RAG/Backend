using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using System;

namespace OJT_RAG.Repositories
{
    public class JobTitleOverviewRepository : IJobTitleOverviewRepository
    {
        private readonly OJTRAGContext _db;

        public JobTitleOverviewRepository(OJTRAGContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<JobTitleOverview>> GetAllAsync()
        {
            return await _db.JobTitleOverviews.ToListAsync();
        }

        public async Task<JobTitleOverview?> GetByIdAsync(long id)
        {
            return await _db.JobTitleOverviews.FirstOrDefaultAsync(x => x.JobTitleId == id);
        }

        public async Task<JobTitleOverview> AddAsync(JobTitleOverview entity)
        {
            await _db.JobTitleOverviews.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<JobTitleOverview> UpdateAsync(JobTitleOverview entity)
        {
            _db.JobTitleOverviews.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var model = await GetByIdAsync(id);
            if (model == null) return false;

            _db.JobTitleOverviews.Remove(model);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
