using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Repositories.Entities;

[Table("job_title_overview")]
public partial class JobTitleOverview
{
    [Key]
    [Column("job_title_id")]
    public long JobTitleId { get; set; }

    [Column("job_title")]
    [StringLength(255)]
    public string? JobTitle { get; set; }

    [Column("position_amount")]
    public int? PositionAmount { get; set; }
}
