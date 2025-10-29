using OJT_RAG.Data;
using OJT_RAG.Models;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Repositories
{
    public class DocumentRepository
    {
        private readonly AppDbContext _context;

        public DocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Document> AddAsync(Document document)
        {
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
            return document;
        }
    }
}
