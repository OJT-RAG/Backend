using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OJT_RAG.Repositories.Entities
{
    [Table("ojtdocumenttag")]
    public class Ojtdocumenttag
    {
        [Key, Column("ojtdocument_id", Order = 0)]
        public long OjtDocumentId { get; set; }

        [Key, Column("documenttag_id", Order = 1)]
        public long DocumentTagId { get; set; }

        [ForeignKey("OjtDocumentId")]
        public virtual Ojtdocument OjtDocument { get; set; } = null!;

        [ForeignKey("DocumentTagId")]
        public virtual Documenttag DocumentTag { get; set; } = null!;
    }
}