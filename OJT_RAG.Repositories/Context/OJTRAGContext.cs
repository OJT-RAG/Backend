using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Enums;

namespace OJT_RAG.Repositories.Context
{

    public partial class OJTRAGContext : DbContext
    {
        static OJTRAGContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<DocumentTagType>("document_tag_type_enum");
            //NpgsqlConnection.GlobalTypeMapper.MapEnum<AccountStatusEnum>("account_status_enum");
            NpgsqlConnection.GlobalTypeMapper.EnableUnmappedTypes();
        }
        public OJTRAGContext() {
        
        }

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
            var envConnection = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (!optionsBuilder.IsConfigured)
            {
                // Chuỗi này CHỈ dùng cho Migration ở máy Local
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ojt_rag;Username=postgres;Password=12345");
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<AccountStatusEnum>("account_status_enum");
            modelBuilder.HasPostgresEnum<DocumentTagType>("document_tag_type_enum");
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.AccountStatus)
                      .HasColumnType("account_status_enum") // 🔥 BẮT BUỘC
                     //.HasConversion(
                     //     v => v.ToString(),                  // enum → string
                     //     v => Enum.Parse<AccountStatusEnum>(v) // string → enum
                     // )
                      .IsRequired();

                entity.Property(e => e.Role)
                      .HasMaxLength(20);
            });

            modelBuilder.Entity<JobPosition>(entity =>
            {
                entity.HasOne(j => j.SemesterCompany)
                      .WithMany(sc => sc.JobPositions)
                      .HasForeignKey(j => j.SemesterCompanyId)
                      .OnDelete(DeleteBehavior.Restrict);   

            });

            modelBuilder.Entity<Documenttag>(entity =>
            {
                entity.HasMany(d => d.OjtDocumentTags)
            .WithOne(odt => odt.DocumentTag)
            .HasForeignKey(odt => odt.DocumentTagId)
            .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(d => d.CompanyDocumentTags)
                      .WithOne(cdt => cdt.DocumentTag)
                      .HasForeignKey(cdt => cdt.DocumentTagId)
                      .OnDelete(DeleteBehavior.Cascade);
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