using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using System;
using OJT_RAG.Repositories.Context;
namespace OJT_RAG.Repositories
{
    public class SemesterCompanyRepository : ISemesterCompanyRepository
    {
        private readonly OJTRAGContext _db;

        public SemesterCompanyRepository(OJTRAGContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<SemesterCompany>> GetAllAsync()
        {
            return await _db.SemesterCompanies.ToListAsync();
        }

        public async Task<SemesterCompany?> GetByIdAsync(long id)
        {
            return await _db.SemesterCompanies
                .FirstOrDefaultAsync(x => x.SemesterCompanyId == id);
        }

        public async Task<IEnumerable<SemesterCompany>> GetBySemesterIdAsync(long semesterId)
        {
            return await _db.SemesterCompanies
                .Where(x => x.SemesterId == semesterId)
                .ToListAsync();
        }

        public async Task<IEnumerable<SemesterCompany>> GetByCompanyIdAsync(long companyId)
        {
            return await _db.SemesterCompanies
                .Where(x => x.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<SemesterCompany> AddAsync(SemesterCompany entity)
        {
            await _db.SemesterCompanies.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<SemesterCompany> UpdateAsync(SemesterCompany entity)
        {
            _db.SemesterCompanies.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var sc = await GetByIdAsync(id);
            if (sc == null) return false;

            _db.SemesterCompanies.Remove(sc);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
