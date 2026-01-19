using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;

namespace OJT_RAG.Repositories
{
    public class OjtDocumentTagRepository : IOjtDocumentTagRepository
    {
        private readonly OJTRAGContext _db;

        public OjtDocumentTagRepository(OJTRAGContext db)
        {
            _db = db;
        }

        public async Task<bool> ExistsAsync(long docId, long tagId)
        {
            return await _db.Ojtdocumenttags
                .AnyAsync(x => x.OjtDocumentId == docId && x.DocumentTagId == tagId);
        }
        public async Task<IEnumerable<Documenttag>> GetTagsByDocumentId(long ojtDocumentId)
        {
            return await _db.Ojtdocumenttags
                .Where(x => x.OjtDocumentId == ojtDocumentId)
                .Include(x => x.DocumentTag)
                .Select(x => x.DocumentTag!)
                .ToListAsync();
        }

        public async Task AddAsync(Ojtdocumenttag entity)
        {
            await _db.Ojtdocumenttags.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveAsync(long docId, long tagId)
        {
            var entity = await _db.Ojtdocumenttags
                .FirstOrDefaultAsync(x => x.OjtDocumentId == docId && x.DocumentTagId == tagId);

            if (entity != null)
            {
                _db.Ojtdocumenttags.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
