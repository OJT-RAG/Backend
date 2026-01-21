using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OJT_RAG.Repositories.Entities
{
    [Table("documenttag")]
    public partial class Documenttag
    {
        [Key]
        [Column("documenttag_id")]
        public long DocumenttagId { get; set; }

        [Column("name")]
        [StringLength(255)]
        public string? Name { get; set; }

        [Column("type")]
        public string? Type { get; set; }
        // Companydocumenttag (N-N qua trung gian)
        [InverseProperty("DocumentTag")]
        public virtual ICollection<Companydocumenttag> CompanyDocumentTags { get; set; }
            = new List<Companydocumenttag>();

        // Ojtdocumenttag (N-N qua trung gian)
        [InverseProperty("DocumentTag")]
        public virtual ICollection<Ojtdocumenttag> OjtDocumentTags { get; set; }
            = new List<Ojtdocumenttag>();
    }
}
