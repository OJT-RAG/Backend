using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Context;

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
        => optionsBuilder.UseNpgsql("Host=localhost;Database=OJT_RAG;Username=postgres;Password=123456");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("account_status_enum", new[] { "active", "inactive" })
            .HasPostgresEnum("chat_room_status_enum", new[] { "active", "inactive" })
            .HasPostgresEnum("document_tag_type_enum", new[] { "company", "university", "system" })
            .HasPostgresEnum("final_report_status_enum", new[] { "not_yet", "finished" })
            .HasPostgresEnum("job_level_enum", new[] { "intern", "fresher", "junior", "middle", "senior", "pm" })
            .HasPostgresEnum("job_title_field_enum", new[] { "IT", "Marketing", "Economy", "Language", "Finance", "Tourism", "Logistics", "Media", "Design", "Automotive" })
            .HasPostgresEnum("major_status_enum", new[] { "active", "inactive" })
            .HasPostgresEnum("ojt_status_enum", new[] { "not_yet", "ongoing", "finished" })
            .HasPostgresEnum("semester_company_status_enum", new[] { "active", "inactive" });

        modelBuilder.Entity<ChatRoom>(entity =>
        {
            entity.HasKey(e => e.ChatRoomId).HasName("chat_room_pkey");

            entity.HasOne(d => d.User).WithMany(p => p.ChatRooms).HasConstraintName("chat_room_user_id_fkey");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("company_pkey");

            entity.HasOne(d => d.Major).WithMany(p => p.Companies).HasConstraintName("company_majorid_fkey");
        });

        modelBuilder.Entity<Companydocument>(entity =>
        {
            entity.HasKey(e => e.CompanydocumentId).HasName("companydocument_pkey");

            entity.HasOne(d => d.SemesterCompany).WithMany(p => p.Companydocuments).HasConstraintName("companydocument_semester_company_id_fkey");

            entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.Companydocuments).HasConstraintName("companydocument_uploaded_by_fkey");

            entity.HasMany(d => d.Documenttags).WithMany(p => p.Companydocuments)
                .UsingEntity<Dictionary<string, object>>(
                    "Companydocumenttag",
                    r => r.HasOne<Documenttag>().WithMany()
                        .HasForeignKey("DocumenttagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("companydocumenttag_documenttag_id_fkey"),
                    l => l.HasOne<Companydocument>().WithMany()
                        .HasForeignKey("CompanydocumentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("companydocumenttag_companydocument_id_fkey"),
                    j =>
                    {
                        j.HasKey("CompanydocumentId", "DocumenttagId").HasName("companydocumenttag_pkey");
                        j.ToTable("companydocumenttag");
                        j.IndexerProperty<long>("CompanydocumentId").HasColumnName("companydocument_id");
                        j.IndexerProperty<long>("DocumenttagId").HasColumnName("documenttag_id");
                    });
        });

        modelBuilder.Entity<Documenttag>(entity =>
        {
            entity.HasKey(e => e.DocumenttagId).HasName("documenttag_pkey");
        });

        modelBuilder.Entity<Finalreport>(entity =>
        {
            entity.HasKey(e => e.FinalreportId).HasName("finalreport_pkey");

            entity.HasOne(d => d.JobPosition).WithMany(p => p.Finalreports).HasConstraintName("finalreport_job_position_id_fkey");

            entity.HasOne(d => d.Semester).WithMany(p => p.Finalreports).HasConstraintName("finalreport_semester_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Finalreports).HasConstraintName("finalreport_user_id_fkey");
        });

        modelBuilder.Entity<JobBookmark>(entity =>
        {
            entity.HasKey(e => e.JobBookmarkId).HasName("job_bookmark_pkey");

            entity.HasOne(d => d.JobPosition).WithMany(p => p.JobBookmarks).HasConstraintName("job_bookmark_job_position_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.JobBookmarks).HasConstraintName("job_bookmark_user_id_fkey");
        });

        modelBuilder.Entity<JobDescription>(entity =>
        {
            entity.HasKey(e => e.JobDescriptionId).HasName("job_description_pkey");

            entity.HasOne(d => d.JobPosition).WithMany(p => p.JobDescriptions).HasConstraintName("job_description_job_position_id_fkey");
        });

        modelBuilder.Entity<JobPosition>(entity =>
        {
            entity.HasKey(e => e.JobPositionId).HasName("job_position_pkey");

            entity.HasOne(d => d.Major).WithMany(p => p.JobPositions).HasConstraintName("job_position_major_id_fkey");

            entity.HasOne(d => d.Semester).WithMany(p => p.JobPositions).HasConstraintName("job_position_semester_id_fkey");
        });

        modelBuilder.Entity<JobTitleOverview>(entity =>
        {
            entity.HasKey(e => e.JobTitleId).HasName("job_title_overview_pkey");

            entity.Property(e => e.PositionAmount).HasDefaultValue(0);
        });

        modelBuilder.Entity<Major>(entity =>
        {
            entity.HasKey(e => e.MajorId).HasName("major_pkey");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("message_pkey");

            entity.HasOne(d => d.ChatRoom).WithMany(p => p.Messages).HasConstraintName("message_chat_room_id_fkey");
        });

        modelBuilder.Entity<Ojtdocument>(entity =>
        {
            entity.HasKey(e => e.OjtdocumentId).HasName("ojtdocument_pkey");

            entity.HasOne(d => d.Semester).WithMany(p => p.Ojtdocuments).HasConstraintName("ojtdocument_semester_id_fkey");

            entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.Ojtdocuments).HasConstraintName("ojtdocument_uploaded_by_fkey");

            entity.HasMany(d => d.Documenttags).WithMany(p => p.Ojtdocuments)
                .UsingEntity<Dictionary<string, object>>(
                    "Ojtdocumenttag",
                    r => r.HasOne<Documenttag>().WithMany()
                        .HasForeignKey("DocumenttagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ojtdocumenttag_documenttag_id_fkey"),
                    l => l.HasOne<Ojtdocument>().WithMany()
                        .HasForeignKey("OjtdocumentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ojtdocumenttag_ojtdocument_id_fkey"),
                    j =>
                    {
                        j.HasKey("OjtdocumentId", "DocumenttagId").HasName("ojtdocumenttag_pkey");
                        j.ToTable("ojtdocumenttag");
                        j.IndexerProperty<long>("OjtdocumentId").HasColumnName("ojtdocument_id");
                        j.IndexerProperty<long>("DocumenttagId").HasColumnName("documenttag_id");
                    });
        });

        modelBuilder.Entity<Semester>(entity =>
        {
            entity.HasKey(e => e.SemesterId).HasName("semester_pkey");
        });

        modelBuilder.Entity<SemesterCompany>(entity =>
        {
            entity.HasKey(e => e.SemesterCompanyId).HasName("semester_company_pkey");

            entity.HasOne(d => d.Company).WithMany(p => p.SemesterCompanies).HasConstraintName("semester_company_company_id_fkey");

            entity.HasOne(d => d.Semester).WithMany(p => p.SemesterCompanies).HasConstraintName("semester_company_semester_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("User_pkey");

            entity.HasOne(d => d.Company).WithMany(p => p.Users).HasConstraintName("User_company_id_fkey");

            entity.HasOne(d => d.Major).WithMany(p => p.Users).HasConstraintName("User_major_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
