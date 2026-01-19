using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OJT_RAG.Repositories.Entities
{
    [Table("companydocumenttag")]
    public class Companydocumenttag
    {
        [Column("companydocument_id")]
        public long CompanyDocumentId { get; set; }

        [Column("documenttag_id")]
        public long DocumentTagId { get; set; }

        public virtual Companydocument? CompanyDocument { get; set; }

        public virtual Documenttag? DocumentTag { get; set; }
    }
}
