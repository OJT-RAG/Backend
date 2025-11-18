using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Repositories.Context;
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

        public async Task<IEnumerable<Companydocument>> GetBySemesterCompanyIdAsync(long semId)
        {
            return await _db.Companydocuments
                .Where(x => x.SemesterCompanyId == semId)
                .ToListAsync();
        }

        public async Task<Companydocument> AddAsync(Companydocument doc)
        {
            await _db.Companydocuments.AddAsync(doc);
            await _db.SaveChangesAsync();
            return doc;
        }

        public async Task<Companydocument> UpdateAsync(Companydocument doc)
        {
            _db.Companydocuments.Update(doc);
            await _db.SaveChangesAsync();
            return doc;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var doc = await GetByIdAsync(id);
            if (doc == null) return false;

            _db.Companydocuments.Remove(doc);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
