using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        //public async Task<IEnumerable<Ojtdocument>> GetByTagTypeAsync(string type)
        //{
        //    if (!Enum.TryParse<DocumentTagType>(type, true, out var enumType))
        //        throw new ArgumentException("Invalid tag type");

        //    return await _db.Ojtdocuments
        //        .Where(o => o.OjtDocumentTags
        //            .Any(odt => odt.DocumentTag.Type == enumType))
        //        .ToListAsync();
        //}
        // Repository
        public async Task<IEnumerable<Ojtdocument>> GetByTagTypeAsync(string type, bool includeRelated = true)
        {
            // ... (phần kiểm tra validValues giữ nguyên)

            var query = _db.Ojtdocuments.AsQueryable();

            // Sửa phần Where này:
            // Chuyển đổi Enum về string ngay trong LINQ để EF/Npgsql xử lý ép kiểu
            query = query.Where(o => o.OjtDocumentTags
                .Any(odt => ((string)(object)odt.DocumentTag.Type).ToLower() == type));

            if (includeRelated)
            {
                query = query
                    .Include(o => o.OjtDocumentTags)
                        .ThenInclude(odt => odt.DocumentTag)
                    .Include(o => o.UploadedByNavigation);
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
