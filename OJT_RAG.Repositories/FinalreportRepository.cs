using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using System;

namespace OJT_RAG.Repositories
{
    public class FinalreportRepository : IFinalreportRepository
    {
        private readonly OJTRAGContext _db;

        public FinalreportRepository(OJTRAGContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Finalreport>> GetAllAsync()
        {
            return await _db.Finalreports.ToListAsync();
        }

        public async Task<Finalreport?> GetByIdAsync(long id)
        {
            return await _db.Finalreports.FirstOrDefaultAsync(x => x.FinalreportId == id);
        }

        public async Task<IEnumerable<Finalreport>> GetByUserIdAsync(long userId)
        {
            return await _db.Finalreports.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Finalreport>> GetBySemesterIdAsync(long semesterId)
        {
            return await _db.Finalreports.Where(x => x.SemesterId == semesterId).ToListAsync();
        }

        public async Task<IEnumerable<Finalreport>> GetByJobPositionIdAsync(long jobPositionId)
        {
            return await _db.Finalreports.Where(x => x.JobPositionId == jobPositionId).ToListAsync();
        }

        public async Task<Finalreport> AddAsync(Finalreport entity)
        {
            await _db.Finalreports.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Finalreport> UpdateAsync(Finalreport entity)
        {
            _db.Finalreports.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;
            _db.Finalreports.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
