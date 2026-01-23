using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Enums;
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

        public async Task<IEnumerable<Ojtdocument>> GetByTagTypeAsync(string type, bool includeRelated = true)
        {
            // Chuyển về lowercase để so sánh chính xác với DB
            string searchType = type.ToLower();

            var query = _db.Ojtdocuments.AsQueryable();

            // Thay vì so sánh Enum object, ta ép kiểu Type về string để so sánh
            // EF Core sẽ dịch cái này sang SQL dùng toán tử ép kiểu của Postgres
            query = query.Where(o => o.OjtDocumentTags
                         .Any(odt => (string)(object)odt.DocumentTag.Type == searchType));

            if (includeRelated)
            {
                query = query
                    .Include(o => o.UploadedByNavigation)
                    .Include(o => o.OjtDocumentTags)
                        .ThenInclude(odt => odt.DocumentTag);
            }

            return await query.ToListAsync();
        }

        public async Task<Ojtdocument> AddAsync(Ojtdocument entity)
        {
            _db.Ojtdocuments.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }


        public async Task<Ojtdocument> UpdateAsync(Ojtdocument entity)
        {
            //_db.Ojtdocuments.Update(entity);
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
