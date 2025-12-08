using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Context
{
    public partial class OJTRAGContext : DbContext
    {
        public OJTRAGContext()
        {
        }

        public OJTRAGContext(DbContextOptions<OJTRAGContext> options)
            : base(options)
        {
        }

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
        public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=ojt_rag;Username=postgres;Password=12345");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ENUMS
            modelBuilder
                .HasPostgresEnum("account_status_enum", new[] { "active", "inactive" })
                .HasPostgresEnum("chat_room_status_enum", new[] { "active", "inactive" })
                .HasPostgresEnum("document_tag_type_enum", new[] { "company", "university", "system" })
                .HasPostgresEnum("final_report_status_enum", new[] { "not_yet", "finished" })
                .HasPostgresEnum("job_level_enum", new[] { "intern", "fresher", "junior", "middle", "senior", "pm" })
                .HasPostgresEnum("job_title_field_enum", new[] { "IT", "Marketing", "Economy", "Language", "Finance", "Tourism", "Logistics", "Media", "Design", "Automotive" })
                .HasPostgresEnum("major_status_enum", new[] { "active", "inactive" })
                .HasPostgresEnum("ojt_status_enum", new[] { "not_yet", "ongoing", "finished" })
                .HasPostgresEnum("semester_company_status_enum", new[] { "active", "inactive" })
                .HasPostgresEnum("user_role_enum", new[] { "admin", "cro_staff", "student", "company" });


            modelBuilder.Entity<ChatRoom>(entity =>
            {
                entity.HasKey(e => e.ChatRoomId);
                entity.HasOne(d => d.User)
                      .WithMany(p => p.ChatRooms)
                      .HasConstraintName("chat_room_user_id_fkey");
            });


            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.CompanyId);
                entity.HasOne(d => d.Major)
                      .WithMany(p => p.Companies)
                      .HasConstraintName("company_majorid_fkey");
            });


            modelBuilder.Entity<Companydocument>(entity =>
            {
                entity.HasKey(e => e.CompanydocumentId);

                entity.HasOne(d => d.SemesterCompany)
                      .WithMany(p => p.Companydocuments)
                      .HasConstraintName("companydocument_semester_company_id_fkey");

                entity.HasOne(d => d.UploadedByNavigation)
                      .WithMany(p => p.Companydocuments)
                      .HasConstraintName("companydocument_uploaded_by_fkey");
            });


            modelBuilder.Entity<Companydocumenttag>(entity =>
            {
                entity.HasKey(e => e.CompanyDocumentTagId).HasName("companydocumenttag_pkey");

                entity.HasOne(e => e.CompanyDocument)
                    .WithMany(d => d.CompanyDocumentTags)
                    .HasForeignKey(e => e.CompanyDocumentId)
                    .HasConstraintName("companydocumenttag_companydocument_id_fkey");

                entity.HasOne(e => e.DocumentTag)
                    .WithMany(d => d.CompanyDocumentTags)
                    .HasForeignKey(e => e.DocumentTagId)
                    .HasConstraintName("companydocumenttag_documenttag_id_fkey");
            });


            modelBuilder.Entity<Documenttag>(entity =>
            {
                entity.HasKey(e => e.DocumenttagId);
            });

            modelBuilder.Entity<Finalreport>(entity =>
            {
                entity.HasKey(e => e.FinalreportId);

                entity.HasOne(d => d.JobPosition).WithMany(p => p.Finalreports);
                entity.HasOne(d => d.Semester).WithMany(p => p.Finalreports);
                entity.HasOne(d => d.User).WithMany(p => p.Finalreports);
            });


            modelBuilder.Entity<JobBookmark>(entity =>
            {
                entity.HasKey(e => e.JobBookmarkId);

                entity.HasOne(d => d.JobPosition).WithMany(p => p.JobBookmarks);
                entity.HasOne(d => d.User).WithMany(p => p.JobBookmarks);
            });


            modelBuilder.Entity<JobDescription>(entity =>
            {
                entity.HasKey(e => e.JobDescriptionId);
                entity.HasOne(d => d.JobPosition).WithMany(p => p.JobDescriptions);
            });


            modelBuilder.Entity<JobPosition>(entity =>
            {
                entity.HasKey(e => e.JobPositionId);

                entity.HasOne(d => d.Major).WithMany(p => p.JobPositions);
                entity.HasOne(d => d.Semester).WithMany(p => p.JobPositions);
            });

            modelBuilder.Entity<JobTitleOverview>(entity =>
            {
                entity.HasKey(e => e.JobTitleId);
                entity.Property(e => e.PositionAmount).HasDefaultValue(0);
            });


            modelBuilder.Entity<Major>(entity =>
            {
                entity.HasKey(e => e.MajorId);
            });


            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.MessageId);
                entity.HasOne(d => d.ChatRoom)
                      .WithMany(p => p.Messages);
            });


            modelBuilder.Entity<Ojtdocument>(entity =>
            {
                entity.HasKey(e => e.OjtdocumentId);

                entity.HasOne(d => d.Semester)
                      .WithMany(p => p.Ojtdocuments)
                      .HasConstraintName("ojtdocument_semester_id_fkey");

                entity.HasOne(d => d.UploadedByNavigation)
                      .WithMany(p => p.Ojtdocuments)
                      .HasConstraintName("ojtdocument_uploaded_by_fkey");
            });


            modelBuilder.Entity<Ojtdocumenttag>(entity =>
            {
                entity.HasKey(e => e.OjtDocumentTagId).HasName("ojtdocumenttag_pkey");

                entity.HasOne(e => e.OjtDocument)
                      .WithMany(d => d.OjtDocumentTags)
                      .HasForeignKey(e => e.OjtDocumentId)
                      .HasConstraintName("ojtdocumenttag_ojtdocument_id_fkey");

                entity.HasOne(e => e.DocumentTag)
                      .WithMany(d => d.OjtDocumentTags)
                      .HasForeignKey(e => e.DocumentTagId)
                      .HasConstraintName("ojtdocumenttag_documenttag_id_fkey");
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.HasKey(e => e.SemesterId);
            });



            modelBuilder.Entity<SemesterCompany>(entity =>
            {
                entity.HasKey(e => e.SemesterCompanyId);

                entity.HasOne(d => d.Company).WithMany(p => p.SemesterCompanies);
                entity.HasOne(d => d.Semester).WithMany(p => p.SemesterCompanies);
            });


            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.Role)
                    .HasColumnType("user_role_enum")
                    .HasConversion(
                        v => v.HasValue ? v.Value switch
                        {
                            UserRole.Admin => "admin",
                            UserRole.CroStaff => "cro_staff",
                            UserRole.Student => "student",
                            UserRole.Company => "company",
                            _ => throw new ArgumentException($"Unknown enum value: {v}")
                        } : (string?)null,
                        v => !string.IsNullOrEmpty(v) ? v switch
                        {
                            "admin" => UserRole.Admin,
                            "cro_staff" => UserRole.CroStaff,
                            "student" => UserRole.Student,
                            "company" => UserRole.Company,
                            _ => throw new ArgumentException($"Unknown database value: {v}")
                        } : (UserRole?)null);

                entity.HasOne(d => d.Company).WithMany(p => p.Users);
                entity.HasOne(d => d.Major).WithMany(p => p.Users);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
