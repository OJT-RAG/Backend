using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OJT_RAG.Models;

namespace OJT_RAG.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cv> Cvs { get; set; }
        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Khai báo enum PostgreSQL
            modelBuilder.HasPostgresEnum<DocumentStatus>();

            // Map Document entity
            modelBuilder.Entity<Document>(entity =>
            {
                entity.Property(e => e.Status)
                      .HasColumnType("document_status")
                      .HasConversion(
                          v => v.ToString(),                   // enum -> string khi lưu DB
                          v => (DocumentStatus)Enum.Parse(typeof(DocumentStatus), v) // string -> enum khi đọc DB
                      );
            });
        }
    }
}
