using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;

namespace OJT_RAG.Repositories
{
    public class OjtDocumentRepository : IOjtDocumentRepository
    {
        private readonly OJTRAGContext _db;

        public OjtDocumentRepository(OJTRAGContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Ojtdocument>> GetAllAsync()
        {
            return await _db.Ojtdocuments.ToListAsync();
        }

        public async Task<Ojtdocument?> GetByIdAsync(long id)
        {
            return await _db.Ojtdocuments.FirstOrDefaultAsync(x => x.OjtdocumentId == id);
        }

        public async Task<IEnumerable<Ojtdocument>> GetBySemesterAsync(long semesterId)
        {
            return await _db.Ojtdocuments.Where(x => x.SemesterId == semesterId).ToListAsync();
        }

        public async Task<Ojtdocument> AddAsync(Ojtdocument entity)
        {
            await _db.Ojtdocuments.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Ojtdocument> UpdateAsync(Ojtdocument entity)
        {
            _db.Ojtdocuments.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var doc = await GetByIdAsync(id);
            if (doc == null) return false;

            _db.Ojtdocuments.Remove(doc);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
