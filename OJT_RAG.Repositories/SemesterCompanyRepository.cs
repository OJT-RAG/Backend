using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using System;

namespace OJT_RAG.Repositories
{
    public class SemesterCompanyRepository : ISemesterCompanyRepository
    {
        private readonly OJTRAGContext _context;

        public SemesterCompanyRepository(OJTRAGContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SemesterCompany>> GetAllAsync()
        {
            return await _context.SemesterCompanies.ToListAsync();
        }

        public async Task<SemesterCompany?> GetByIdAsync(long id)
        {
            return await _context.SemesterCompanies.FindAsync(id);
        }

        public async Task<IEnumerable<SemesterCompany>> GetBySemesterIdAsync(long semesterId)
        {
            return await _context.SemesterCompanies
                .Where(x => x.SemesterId == semesterId)
                .ToListAsync();
        }

        public async Task<IEnumerable<SemesterCompany>> GetByCompanyIdAsync(long companyId)
        {
            return await _context.SemesterCompanies
                .Where(x => x.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(long semesterId, long companyId)
        {
            return await _context.SemesterCompanies
                .AnyAsync(x => x.SemesterId == semesterId && x.CompanyId == companyId);
        }

        public async Task AddAsync(SemesterCompany entity)
        {
            _context.SemesterCompanies.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SemesterCompany entity)
        {
            _context.SemesterCompanies.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var x = await _context.SemesterCompanies.FindAsync(id);
            if (x == null) return false;

            _context.SemesterCompanies.Remove(x);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
