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

        public async Task<IEnumerable<Ojtdocumenttag>> GetAllAsync()
        {
            return await _db.Ojtdocumenttags
                .Include(x => x.OjtDocument)
                .Include(x => x.DocumentTag)
                .ToListAsync();
        }

        public async Task<Ojtdocumenttag?> GetByIdAsync(long id)
        {
            return await _db.Ojtdocumenttags
                .FirstOrDefaultAsync(x => x.OjtDocumentTagId == id);
        }

        public async Task AddAsync(Ojtdocumenttag entity)
        {
            await _db.Ojtdocumenttags.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ojtdocumenttag entity)
        {
            _db.Ojtdocumenttags.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var x = await _db.Ojtdocumenttags.FindAsync(id);
            if (x == null) return false;

            _db.Ojtdocumenttags.Remove(x);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
