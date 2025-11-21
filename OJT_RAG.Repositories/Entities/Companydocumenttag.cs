using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OJT_RAG.Repositories.Entities
{
    [Table("companydocumenttag")]
    public class Companydocumenttag
    {
        [Key]
        [Column("companydocumenttag_id")]
        public long CompanyDocumentTagId { get; set; }

        [Column("companydocument_id")]
        public long CompanyDocumentId { get; set; }

        [Column("documenttag_id")]
        public long DocumentTagId { get; set; }

        [ForeignKey("CompanyDocumentId")]
        public virtual Companydocument? CompanyDocument { get; set; }

        [ForeignKey("DocumentTagId")]
        public virtual Documenttag? DocumentTag { get; set; }
    }
}
