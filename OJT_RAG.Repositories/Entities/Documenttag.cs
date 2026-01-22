using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OJT_RAG.Repositories.Enums;

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
        public DocumentTagType Type { get; set; }

        // Quan hệ nhiều-nhiều với CompanyDocument qua bảng trung gian
        [InverseProperty("DocumentTag")]
        public virtual ICollection<Companydocumenttag> CompanyDocumentTags { get; set; }
            = new List<Companydocumenttag>();

        // Quan hệ nhiều-nhiều với OJTDocument qua bảng trung gian
        [InverseProperty("DocumentTag")]
        public virtual ICollection<Ojtdocumenttag> OjtDocumentTags { get; set; }
            = new List<Ojtdocumenttag>();
    }
}