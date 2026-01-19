using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Repositories.Entities;

[Table("semester_company")]
public partial class SemesterCompany
{
    [Key]
    [Column("semester_company_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long SemesterCompanyId { get; set; }

    [Column("semester_id")]
    public long? SemesterId { get; set; }

    [Column("company_id")]
    public long? CompanyId { get; set; }

    [Column("approved_at", TypeName = "timestamp with time zone")]
    public DateTime? ApprovedAt { get; set; }

    // ── Relationships ───────────────────────────────────────────────

    [ForeignKey("CompanyId")]
    [InverseProperty("SemesterCompanies")]
    public virtual Company? Company { get; set; }

    [ForeignKey("SemesterId")]
    [InverseProperty("SemesterCompanies")]
    public virtual Semester? Semester { get; set; }

    [InverseProperty("SemesterCompany")]
    public virtual ICollection<Companydocument> Companydocuments { get; set; } = new List<Companydocument>();

    // ── MỚI: Relationship 2 chiều với JobPosition ──────────────────
    // Cho phép SemesterCompany truy cập danh sách tất cả JobPosition
    // thuộc về công ty này trong kỳ này
    [InverseProperty("SemesterCompany")]
    public virtual ICollection<JobPosition> JobPositions { get; set; } = new List<JobPosition>();
}