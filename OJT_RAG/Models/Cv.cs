using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OJT_RAG.Models
{
    [Table("cvs")] // tên bảng trong PostgreSQL
    public class Cv
    {
        [Key]
        [Column("cvid")]
        public int CvId { get; set; }

        [Column("userid")]
        public int UserId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("filename")]
        public string FileName { get; set; }

        [Column("linkurl")]
        public string LinkUrl { get; set; }

        [Column("uploaddate")]
        public DateTime UploadDate { get; set; }

        [Column("status")]
        public string Status { get; set; }
    }
}
