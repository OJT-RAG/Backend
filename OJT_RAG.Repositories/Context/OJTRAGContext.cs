using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql;
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Context
{
    //public class UnspecifiedDateTimeConverter : ValueConverter<DateTime, DateTime>
    //{
    //    public UnspecifiedDateTimeConverter()
    //        : base(
    //            v => v,  // convert to provider: giữ nguyên
    //            v => DateTime.SpecifyKind(v, DateTimeKind.Unspecified)) // convert from provider
    //    { }
    //}

    //public class UnspecifiedNullableDateTimeConverter : ValueConverter<DateTime?, DateTime?>
    //{
    //    public UnspecifiedNullableDateTimeConverter()
    //        : base(
    //            v => v,
    //            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Unspecified) : v)
    //    { }
    //}

    public partial class OJTRAGContext : DbContext
    {
        public OJTRAGContext() { }

        public OJTRAGContext(DbContextOptions<OJTRAGContext> options)
            : base(options)
        {
        }
        public override async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                foreach (var prop in entry.Properties)
                {
                    if (prop.Metadata.ClrType == typeof(DateTime))
                    {
                        if (prop.CurrentValue == null) continue;

                        var dt = (DateTime)prop.CurrentValue;

                        if (dt.Kind == DateTimeKind.Local)
                            prop.CurrentValue = dt.ToUniversalTime();
                        else if (dt.Kind == DateTimeKind.Unspecified)
                            prop.CurrentValue = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
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
        public virtual DbSet<UserChatMessage> UserChatMessages { get; set; } // ⭐ THÊM DÒNG NÀY ĐỂ FIX LỖI
        public virtual DbSet<Ojtdocument> Ojtdocuments { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<SemesterCompany> SemesterCompanies { get; set; }
        public virtual DbSet<Companydocumenttag> Companydocumenttags { get; set; }
        public virtual DbSet<Ojtdocumenttag> Ojtdocumenttags { get; set; }
        public virtual DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var envConnection = Environment.GetEnvironmentVariable("DATABASE_URL");
            //if (!optionsBuilder.IsConfigured)
            //{
            //    // Chuỗi này CHỈ dùng cho Migration ở máy Local
            //    optionsBuilder.UseNpgsql("Host=localhost;Database=ojt_rag;Username=postgres;Password=12345");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //// ⭐ Đăng ký PostgreSQL enum cho EF Core
            //modelBuilder.HasPostgresEnum<UserRole>("user_role_enum");

            // ⭐ Fix DateTime Kind cho toàn project
            //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            //{
            //    foreach (var property in entityType.GetProperties())
            //    {
            //        if (property.ClrType == typeof(DateTime))
            //            property.SetValueConverter(new UnspecifiedDateTimeConverter());
            //        else if (property.ClrType == typeof(DateTime?))
            //            property.SetValueConverter(new UnspecifiedNullableDateTimeConverter());
            //    }
            //}

            // ⭐ Cấu hình User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.Role)
                      .HasColumnName("role")
                      //.HasColumnType("user_role_enum")
                      .IsRequired(true);
            });
            modelBuilder.Entity<JobPosition>(entity =>
            {
                entity.HasOne(j => j.SemesterCompany)
                      .WithMany(sc => sc.JobPositions)
                      .HasForeignKey(j => j.SemesterCompanyId)
                      .OnDelete(DeleteBehavior.Restrict);   

            });

            modelBuilder.Entity<Ojtdocumenttag>(entity =>
            {
                entity.HasKey(e => new { e.OjtDocumentId, e.DocumentTagId });

                entity.Property(e => e.OjtDocumentId)
                      .ValueGeneratedNever();

                entity.Property(e => e.DocumentTagId)
                      .ValueGeneratedNever();

                entity.HasOne(e => e.OjtDocument)
                      .WithMany(d => d.OjtDocumentTags)
                      .HasForeignKey(e => e.OjtDocumentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.DocumentTag)
                      .WithMany(t => t.OjtDocumentTags)
                      .HasForeignKey(e => e.DocumentTagId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Companydocumenttag>(entity =>
            {
                entity.HasKey(e => new { e.CompanyDocumentId, e.DocumentTagId });
                entity.Property(e => e.CompanyDocumentId)
                      .ValueGeneratedNever();

                entity.Property(e => e.DocumentTagId)
                      .ValueGeneratedNever();

                entity.HasOne(e => e.CompanyDocument)
                      .WithMany(d => d.CompanyDocumentTags)
                      .HasForeignKey(e => e.CompanyDocumentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.DocumentTag)
                      .WithMany(t => t.CompanyDocumentTags)
                      .HasForeignKey(e => e.DocumentTagId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}