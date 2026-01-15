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
        public async Task<long> GetNextId()
        {
            var maxId = await _context.Companies.MaxAsync(c => (long?)c.CompanyId) ?? 0;
            return maxId + 1;
        }

        // Lấy tất cả Company + kèm navigation (nếu cần)
        public async Task<IEnumerable<Company>> GetAll()
        {
            return await _context.Companies
                .Include(c => c.Major)
                .Include(c => c.Users)
                .Include(c => c.SemesterCompanies)
                .ToListAsync();
        }

        // Lấy theo ID
        public async Task<Company?> GetById(long id)
        {
            return await _context.Companies
                .Include(c => c.Major)
                .Include(c => c.Users)
                .Include(c => c.SemesterCompanies)
                .FirstOrDefaultAsync(c => c.CompanyId == id);
        }

        // Thêm mới
        public async Task Add(Company entity)
        {
            await _context.Companies.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // Update an toàn
        public async Task Update(Company entity)
        {
            var existing = await _context.Companies.FindAsync(entity.CompanyId);

            if (existing == null)
                throw new KeyNotFoundException($"Company {entity.CompanyId} không tồn tại.");

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        // Xoá
        public async Task Delete(long id)
        {
            var item = await _context.Companies.FindAsync(id);

            if (item == null)
                return;

            _context.Companies.Remove(item);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(long id)
        {
            return await _context.Companies.AnyAsync(x => x.CompanyId == id);
        }

    }
}
