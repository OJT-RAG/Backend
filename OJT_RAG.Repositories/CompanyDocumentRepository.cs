using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using System;

namespace OJT_RAG.Repositories
{
    public class CompanyDocumentRepository : ICompanyDocumentRepository
    {
        private readonly OJTRAGContext _db;

        public CompanyDocumentRepository(OJTRAGContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Companydocument>> GetAllAsync()
        {
            return await _db.Companydocuments.ToListAsync();
        }

        public async Task<Companydocument?> GetByIdAsync(long id)
        {
            return await _db.Companydocuments
                .FirstOrDefaultAsync(x => x.CompanydocumentId == id);
        }

        public async Task<IEnumerable<Companydocument>> GetBySemesterCompanyIdAsync(long semCompanyId)
        {
            return await _db.Companydocuments
                .Where(x => x.SemesterCompanyId == semCompanyId)
                .ToListAsync();
        }

        public async Task AddAsync(Companydocument entity)
        {
            _db.Companydocuments.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Companydocument entity)
        {
            _db.Companydocuments.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _db.Companydocuments.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
