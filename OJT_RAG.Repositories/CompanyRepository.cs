using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;

namespace OJT_RAG.Repositories.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly OJTRAGContext _context;

        public CompanyRepository(OJTRAGContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> GetAll()
            => await _context.Companies.ToListAsync();

        public async Task<Company?> GetById(long id)
            => await _context.Companies.FindAsync(id);

        public async Task Add(Company entity)
        {
            await _context.Companies.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Company entity)
        {
            _context.Companies.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var item = await _context.Companies.FindAsync(id);
            if (item != null)
            {
                _context.Companies.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
