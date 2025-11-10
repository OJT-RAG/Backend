using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Repositories.Entities;

[Table("major")]
public partial class Major
{
    [Key]
    [Column("major_id")]
    public long MajorId { get; set; }

    [Column("major_title")]
    [StringLength(255)]
    public string? MajorTitle { get; set; }

    [Column("major_code")]
    [StringLength(100)]
    public string? MajorCode { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("create_at", TypeName = "timestamp without time zone")]
    public DateTime? CreateAt { get; set; }

    [Column("update_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdateAt { get; set; }

    [InverseProperty("Major")]
    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    [InverseProperty("Major")]
    public virtual ICollection<JobPosition> JobPositions { get; set; } = new List<JobPosition>();

    [InverseProperty("Major")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
