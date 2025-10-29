using Microsoft.EntityFrameworkCore;
using OJT_RAG.Models;

namespace OJT_RAG.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cv> Cvs { get; set; }
      
    }
}
