using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using System;

namespace OJT_RAG.Repositories
{
    public class DocumentTagRepository : IDocumentTagRepository
    {
        private readonly OJTRAGContext _db;

        public DocumentTagRepository(OJTRAGContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Documenttag>> GetAllAsync()
        {
            return await _db.Documenttags.ToListAsync();
        }

        public async Task<Documenttag?> GetByIdAsync(long id)
        {
            return await _db.Documenttags.FirstOrDefaultAsync(x => x.DocumenttagId == id);
        }

        public async Task<Documenttag> AddAsync(Documenttag entity)
        {
            await _db.Documenttags.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Documenttag> UpdateAsync(Documenttag entity)
        {
            _db.Documenttags.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var tag = await GetByIdAsync(id);
            if (tag == null) return false;

            _db.Documenttags.Remove(tag);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
