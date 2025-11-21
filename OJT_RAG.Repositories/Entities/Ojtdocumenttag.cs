using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OJT_RAG.Repositories.Entities
{
    [Table("ojtdocumenttag")]
    public class Ojtdocumenttag
    {
        [Key]
        [Column("ojtdocumenttag_id")]
        public long OjtDocumentTagId { get; set; }

        [Column("ojtdocument_id")]
        public long OjtDocumentId { get; set; }

        [Column("documenttag_id")]
        public long DocumentTagId { get; set; }

        [ForeignKey("OjtDocumentId")]
        public virtual Ojtdocument? OjtDocument { get; set; }

        [ForeignKey("DocumentTagId")]
        public virtual Documenttag? DocumentTag { get; set; }
    }
}
