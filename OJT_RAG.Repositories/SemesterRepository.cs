using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using System;

namespace OJT_RAG.Repositories
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly OJTRAGContext _context;

        public SemesterRepository(OJTRAGContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Semester>> GetAll()
            => await _context.Semesters.ToListAsync();

        public async Task<Semester?> GetById(long id)
            => await _context.Semesters.FindAsync(id);

        public async Task<long> GetNextId()
        {
            var maxId = await _context.Semesters.MaxAsync(s => (long?)s.SemesterId) ?? 0;
            return maxId + 1;
        }

        public async Task Add(Semester entity)
        {
            _context.Semesters.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Semester entity)
        {
            _context.Semesters.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var entity = await GetById(id);
            if (entity != null)
            {
                _context.Semesters.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> ExistsAsync(long id)
        {
            return await _context.Semesters.AnyAsync(x => x.SemesterId == id);
        }

    }
}
