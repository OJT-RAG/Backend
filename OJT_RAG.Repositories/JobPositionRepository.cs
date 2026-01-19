using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Repositories.Context;
using System;

namespace OJT_RAG.Repositories
{
    public class JobPositionRepository : IJobPositionRepository
    {
        private readonly OJTRAGContext _db;

        public JobPositionRepository(OJTRAGContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<JobPosition>> GetAllAsync()
        {
            return await _db.JobPositions.ToListAsync();
        }

        public async Task<JobPosition?> GetByIdAsync(long id)
        {
            return await _db.JobPositions.FirstOrDefaultAsync(x => x.JobPositionId == id);
        }

        public async Task<JobPosition> AddAsync(JobPosition entity)
        {
            await _db.JobPositions.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<JobPosition> UpdateAsync(JobPosition entity)
        {
            _db.JobPositions.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var model = await GetByIdAsync(id);
            if (model == null) return false;

            _db.JobPositions.Remove(model);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> HasJobApplicationAsync(long jobPositionId)
        {
            return await _db.JobApplications
                .AnyAsync(x => x.JobPositionId == jobPositionId);
        }

    }
}
