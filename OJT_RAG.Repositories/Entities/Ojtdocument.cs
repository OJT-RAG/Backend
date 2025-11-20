using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OJT_RAG.Repositories.Entities;

[Table("ojtdocument")]
public partial class Ojtdocument
{
    [Key]
    [Column("ojtdocument_id")]
    public long OjtdocumentId { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string? Title { get; set; }

    [Column("file_url")]
    [StringLength(255)]
    public string? FileUrl { get; set; }

    [Column("semester_id")]
    public long? SemesterId { get; set; }

    [Column("is_general")]
    public bool? IsGeneral { get; set; }

    [Column("uploaded_by")]
    public long? UploadedBy { get; set; }

    [Column("uploaded_at", TypeName = "timestamp without time zone")]
    public DateTime? UploadedAt { get; set; }

    [ForeignKey("SemesterId")]
    public virtual Semester? Semester { get; set; }

    [ForeignKey("UploadedBy")]
    public virtual User? UploadedByNavigation { get; set; }

    public virtual ICollection<Documenttag> Documenttags { get; set; } = new List<Documenttag>();
}
