using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql;
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Context
{
    // ⭐ Converter: DateTime -> lưu nguyên nhưng luôn đọc về dưới dạng Unspecified
    public class UnspecifiedDateTimeConverter : ValueConverter<DateTime, DateTime>
    {
        public UnspecifiedDateTimeConverter()
            : base(
                v => v,  // convert to provider: giữ nguyên
                v => DateTime.SpecifyKind(v, DateTimeKind.Unspecified)) // convert from provider
        { }
    }

    public class UnspecifiedNullableDateTimeConverter : ValueConverter<DateTime?, DateTime?>
    {
        public UnspecifiedNullableDateTimeConverter()
            : base(
                v => v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Unspecified) : v)
        { }
    }


    public partial class OJTRAGContext : DbContext
    {
        public OJTRAGContext() { }

        public OJTRAGContext(DbContextOptions<OJTRAGContext> options)
            : base(options)
        {
        }

        // ⭐ DBSets
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ChatRoom> ChatRooms { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Companydocument> Companydocuments { get; set; }
        public virtual DbSet<Documenttag> Documenttags { get; set; }
        public virtual DbSet<Finalreport> Finalreports { get; set; }
        public virtual DbSet<JobBookmark> JobBookmarks { get; set; }
        public virtual DbSet<JobDescription> JobDescriptions { get; set; }
        public virtual DbSet<JobPosition> JobPositions { get; set; }
        public virtual DbSet<JobTitleOverview> JobTitleOverviews { get; set; }
        public virtual DbSet<Major> Majors { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Ojtdocument> Ojtdocuments { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<SemesterCompany> SemesterCompanies { get; set; }
        public virtual DbSet<Companydocumenttag> Companydocumenttags { get; set; }
        public virtual DbSet<Ojtdocumenttag> Ojtdocumenttags { get; set; }
        public virtual DbSet<UserChatMessage> UserChatMessages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=ojt_rag;Username=postgres;Password=12345");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //// ⭐ Đăng ký PostgreSQL enum cho EF Core
            //modelBuilder.HasPostgresEnum<UserRole>("user_role_enum");

            // ⭐ Fix DateTime Kind cho toàn project
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                        property.SetValueConverter(new UnspecifiedDateTimeConverter());
                    else if (property.ClrType == typeof(DateTime?))
                        property.SetValueConverter(new UnspecifiedNullableDateTimeConverter());
                }
            }

            // ⭐ Cấu hình User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.Role)
                      .HasColumnName("role")
                      //.HasColumnType("user_role_enum")
                      .IsRequired(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
