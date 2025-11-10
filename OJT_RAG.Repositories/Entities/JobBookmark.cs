using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Repositories.Entities;

[Table("job_bookmark")]
public partial class JobBookmark
{
    [Key]
    [Column("job_bookmark_id")]
    public long JobBookmarkId { get; set; }

    [Column("user_id")]
    public long? UserId { get; set; }

    [Column("job_position_id")]
    public long? JobPositionId { get; set; }

    [Column("create_at", TypeName = "timestamp without time zone")]
    public DateTime? CreateAt { get; set; }

    [ForeignKey("JobPositionId")]
    [InverseProperty("JobBookmarks")]
    public virtual JobPosition? JobPosition { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("JobBookmarks")]
    public virtual User? User { get; set; }
}
