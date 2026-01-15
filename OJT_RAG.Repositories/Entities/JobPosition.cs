using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Repositories.Entities;

[Table("job_position")]
public partial class JobPosition
{
    [Key]
    [Column("job_position_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long JobPositionId { get; set; }

    [Column("major_id")]
    public long? MajorId { get; set; }

    [Column("semester_id")]
    public long? SemesterId { get; set; }

    [Column("semester_company_id")]
    public long? SemesterCompanyId { get; set; }  // Field mới

    [Column("job_title")]
    [StringLength(255)]
    public string? JobTitle { get; set; }

    [Column("requirements")]
    public string? Requirements { get; set; }

    [Column("benefit")]
    public string? Benefit { get; set; }

    [Column("location")]
    [StringLength(255)]
    public string? Location { get; set; }

    [Column("salary_range")]
    [StringLength(255)]
    public string? SalaryRange { get; set; }

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [Column("create_at", TypeName = "timestamp without time zone")]
    public DateTime? CreateAt { get; set; }

    [Column("update_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdateAt { get; set; }

    // Relationships
    [InverseProperty("JobPosition")]
    public virtual ICollection<Finalreport> Finalreports { get; set; } = new List<Finalreport>();

    [InverseProperty("JobPosition")]
    public virtual ICollection<JobBookmark> JobBookmarks { get; set; } = new List<JobBookmark>();

    [InverseProperty("JobPosition")]
    public virtual ICollection<JobDescription> JobDescriptions { get; set; } = new List<JobDescription>();

    [ForeignKey("MajorId")]
    [InverseProperty("JobPositions")]
    public virtual Major? Major { get; set; }

    [ForeignKey("SemesterId")]
    [InverseProperty("JobPositions")]
    public virtual Semester? Semester { get; set; }

    // MỚI: Relationship với SemesterCompany
    [ForeignKey("SemesterCompanyId")]
    public virtual SemesterCompany? SemesterCompany { get; set; }
}