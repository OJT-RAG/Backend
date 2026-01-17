using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Repositories.Entities;

[Table("job_description")]
public partial class JobDescription
{
    [Key]
    [Column("job_description_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long JobDescriptionId { get; set; }

    [Column("job_position_id")]
    public long? JobPositionId { get; set; }

    [Column("job_description")]
    public string? JobDescription1 { get; set; }

    [Column("hire_quantity")]
    public int? HireQuantity { get; set; }

    [Column("applied_quantity")]
    public int? AppliedQuantity { get; set; }

    [ForeignKey("JobPositionId")]
    [InverseProperty("JobDescriptions")]
    public virtual JobPosition? JobPosition { get; set; }
}
