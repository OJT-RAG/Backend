using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OJT_RAG.Repositories.Context
{
    public class OJTRAGContextFactory : IDesignTimeDbContextFactory<OJTRAGContext>
    {
        public OJTRAGContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OJTRAGContext>();

            // Chuỗi kết nối cho migration (thay đổi nếu cần)
            var connectionString = "Host=localhost;Port=5432;Database=ojt_rag;Username=postgres;Password=12345";

            optionsBuilder.UseNpgsql(connectionString);

            return new OJTRAGContext(optionsBuilder.Options);
        }
    }
}
