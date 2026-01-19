using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OJT_RAG.Repositories.Entities
{
    [Table("ojtdocumenttag")]
    public class Ojtdocumenttag
    {
        [Column("ojtdocument_id")]
        public long OjtDocumentId { get; set; }

        [Column("documenttag_id")]
        public long DocumentTagId { get; set; }

        public virtual Ojtdocument? OjtDocument { get; set; }
        public virtual Documenttag? DocumentTag { get; set; }
    }
}
