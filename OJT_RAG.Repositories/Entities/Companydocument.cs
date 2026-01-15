using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Repositories.Entities
{
    [Table("companydocument")]
    public partial class Companydocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("companydocument_id")]
        public long CompanydocumentId { get; set; }

        [Column("semester_company_id")]
        public long? SemesterCompanyId { get; set; }

        [Column("title")]
        [StringLength(255)]
        public string? Title { get; set; }

        [Column("file_url")]
        [StringLength(255)]
        public string? FileUrl { get; set; }

        [Column("uploaded_by")]
        public long? UploadedBy { get; set; }

        [Column("is_public")]
        public bool? IsPublic { get; set; }

       
        [ForeignKey("SemesterCompanyId")]
        [InverseProperty("Companydocuments")]
        public virtual SemesterCompany? SemesterCompany { get; set; }

       
        [ForeignKey("UploadedBy")]
        [InverseProperty("Companydocuments")]
        public virtual User? UploadedByNavigation { get; set; }

      
        [InverseProperty("CompanyDocument")]
        public virtual ICollection<Companydocumenttag> CompanyDocumentTags { get; set; }
            = new List<Companydocumenttag>();
    }
}
