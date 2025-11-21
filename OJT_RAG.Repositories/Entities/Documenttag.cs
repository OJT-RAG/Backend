using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Repositories.Entities;

[Table("documenttag")]
public partial class Documenttag
{
    [Key]
    [Column("documenttag_id")]
    public long DocumenttagId { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string? Name { get; set; }

    [ForeignKey("DocumenttagId")]
    [InverseProperty("Documenttags")]
    public virtual ICollection<Companydocument> Companydocuments { get; set; } = new List<Companydocument>();

    [ForeignKey("DocumenttagId")]
    [InverseProperty("Documenttags")]
    public virtual ICollection<Ojtdocument> Ojtdocuments { get; set; } = new List<Ojtdocument>();

    [InverseProperty("Documenttag")]
    public virtual ICollection<Companydocumenttag> CompanyDocumentTags { get; set; } = new List<Companydocumenttag>();

}
