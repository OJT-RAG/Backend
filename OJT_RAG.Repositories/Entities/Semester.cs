using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Repositories.Entities;

[Table("semester")]
public partial class Semester
{
    [Key]
    [Column("semester_id")]
    public long SemesterId { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string? Name { get; set; }

    [Column("start_date")]
    public DateOnly? StartDate { get; set; }

    [Column("end_date")]
    public DateOnly? EndDate { get; set; }

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [InverseProperty("Semester")]
    public virtual ICollection<Finalreport> Finalreports { get; set; } = new List<Finalreport>();

    [InverseProperty("Semester")]
    public virtual ICollection<JobPosition> JobPositions { get; set; } = new List<JobPosition>();

    [InverseProperty("Semester")]
    public virtual ICollection<Ojtdocument> Ojtdocuments { get; set; } = new List<Ojtdocument>();

    [InverseProperty("Semester")]
    public virtual ICollection<SemesterCompany> SemesterCompanies { get; set; } = new List<SemesterCompany>();
}
