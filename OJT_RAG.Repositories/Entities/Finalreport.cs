using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Repositories.Entities;

[Table("finalreport")]
public partial class Finalreport
{
    [Key]
    [Column("finalreport_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long FinalreportId { get; set; }

    [Column("user_id")]
    public long? UserId { get; set; }

    [Column("job_position_id")]
    public long? JobPositionId { get; set; }

    [Column("semester_id")]
    public long? SemesterId { get; set; }

    [Column("student_report_file")]
    [StringLength(255)]
    public string? StudentReportFile { get; set; }

    [Column("student_report_text")]
    public string? StudentReportText { get; set; }

    [Column("company_feedback")]
    public string? CompanyFeedback { get; set; }

    [Column("company_rating")]
    public int? CompanyRating { get; set; }

    [Column("company_evaluator")]
    [StringLength(255)]
    public string? CompanyEvaluator { get; set; }

    [Column("submitted_at", TypeName = "timestamp without time zone")]
    public DateTime? SubmittedAt { get; set; }

    [Column("evaluated_at", TypeName = "timestamp without time zone")]
    public DateTime? EvaluatedAt { get; set; }

    [ForeignKey("JobPositionId")]
    [InverseProperty("Finalreports")]
    public virtual JobPosition? JobPosition { get; set; }

    [ForeignKey("SemesterId")]
    [InverseProperty("Finalreports")]
    public virtual Semester? Semester { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Finalreports")]
    public virtual User? User { get; set; }
}
