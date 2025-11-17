using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using System;

namespace OJT_RAG.Repositories
{
    public class JobBookmarkRepository : IJobBookmarkRepository
    {
        private readonly OJTRAGContext _db;

        public JobBookmarkRepository(OJTRAGContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<JobBookmark>> GetAllAsync()
        {
            return await _db.JobBookmarks.ToListAsync();
        }

        public async Task<JobBookmark?> GetByIdAsync(long id)
        {
            return await _db.JobBookmarks.FirstOrDefaultAsync(x => x.JobBookmarkId == id);
        }

        public async Task<IEnumerable<JobBookmark>> GetByUserIdAsync(long userId)
        {
            return await _db.JobBookmarks.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<JobBookmark> AddAsync(JobBookmark entity)
        {
            await _db.JobBookmarks.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<JobBookmark> UpdateAsync(JobBookmark entity)
        {
            _db.JobBookmarks.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _db.JobBookmarks.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
