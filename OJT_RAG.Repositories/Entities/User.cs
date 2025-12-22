using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Repositories.Entities;

[Table("User")]
public partial class User
{
    [Key]
    [Column("user_id")]
    public long UserId { get; set; }

    [Column("major_id")]
    public long? MajorId { get; set; }

    [Column("company_id")]
    public long? CompanyId { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string? Email { get; set; }

    [Column("password")]
    [StringLength(255)]
    public string? Password { get; set; }

    [Column("role")]
    [StringLength(20)]
    public string? Role { get; set; }

    [Column("fullname")]
    [StringLength(255)]
    public string? Fullname { get; set; }

    [Column("student_code")]
    [StringLength(100)]
    public string? StudentCode { get; set; }

    [Column("dob")]
    public DateOnly? Dob { get; set; }

    [Column("phone")]
    [StringLength(50)]
    public string? Phone { get; set; }

    [Column("avatar_url")]
    [StringLength(255)]
    public string? AvatarUrl { get; set; }

    [Column("cv_url")]
    [StringLength(255)]
    public string? CvUrl { get; set; }

    [Column("create_at", TypeName = "timestamp without time zone")]
    public DateTime? CreateAt { get; set; }

    [Column("update_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdateAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<ChatRoom> ChatRooms { get; set; } = new List<ChatRoom>();

    [ForeignKey("CompanyId")]
    [InverseProperty("Users")]
    public virtual Company? Company { get; set; }

    [InverseProperty("UploadedByNavigation")]
    public virtual ICollection<Companydocument> Companydocuments { get; set; } = new List<Companydocument>();

    [InverseProperty("User")]
    public virtual ICollection<Finalreport> Finalreports { get; set; } = new List<Finalreport>();

    [InverseProperty("User")]
    public virtual ICollection<JobBookmark> JobBookmarks { get; set; } = new List<JobBookmark>();

    [ForeignKey("MajorId")]
    [InverseProperty("Users")]
    public virtual Major? Major { get; set; }

    [InverseProperty("UploadedByNavigation")]
    public virtual ICollection<Ojtdocument> Ojtdocuments { get; set; } = new List<Ojtdocument>();
}
