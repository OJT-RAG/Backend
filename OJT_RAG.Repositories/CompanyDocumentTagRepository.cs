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
        public async Task<bool> ExistsAsync(long docId, long tagId)
        {
            return await _db.Companydocumenttags
                .AnyAsync(x => x.CompanyDocumentId == docId && x.DocumentTagId == tagId);
        }
        public async Task<IEnumerable<Documenttag>> GetTagsByDocumentId(long companyDocumentId)
        {
            return await _db.Companydocumenttags
                .Where(x => x.CompanyDocumentId == companyDocumentId)
                .Include(x => x.DocumentTag)
                .Select(x => x.DocumentTag!)
                .ToListAsync();
        }

        public async Task AddAsync(Companydocumenttag entity)
        {
            await _db.Companydocumenttags.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveAsync(long docId, long tagId)
        {
            var entity = await _db.Companydocumenttags
                .FirstOrDefaultAsync(x => x.CompanyDocumentId == docId && x.DocumentTagId == tagId);

            if (entity != null)
            {
                _db.Companydocumenttags.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
