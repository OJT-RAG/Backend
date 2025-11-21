using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;

namespace OJT_RAG.Repositories
{
    public class CompanyDocumentTagRepository : ICompanyDocumentTagRepository
    {
        private readonly OJTRAGContext _db;

        public CompanyDocumentTagRepository(OJTRAGContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Companydocumenttag>> GetAllAsync()
        {
            return await _db.Companydocumenttags
                .Include(x => x.CompanyDocument)
                .Include(x => x.DocumentTag)
                .ToListAsync();
        }

        public async Task<Companydocumenttag?> GetByIdAsync(long id)
        {
            return await _db.Companydocumenttags
                .FirstOrDefaultAsync(x => x.CompanyDocumentTagId == id);
        }

        public async Task AddAsync(Companydocumenttag entity)
        {
            await _db.Companydocumenttags.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Companydocumenttag entity)
        {
            _db.Companydocumenttags.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var x = await _db.Companydocumenttags.FindAsync(id);
            if (x == null) return false;

            _db.Companydocumenttags.Remove(x);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
