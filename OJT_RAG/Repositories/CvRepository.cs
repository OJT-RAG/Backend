using OJT_RAG.Data;
using OJT_RAG.Models;


namespace OJT_RAG.Repositories
{
    public interface ICvRepository
    {
        Task AddAsync(Cv cv);
    }

    public class CvRepository : ICvRepository
    {
        private readonly AppDbContext _context;
        public CvRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(Cv cv)
        {
            _context.Cvs.Add(cv);
            await _context.SaveChangesAsync();
        }
    }
}
