using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Nodes;

namespace OJT_RAG.Models
{
    // 🧩 Định nghĩa Enum ngay trong file này
    public enum DocumentStatus
    {
        Draft,
        Published,
        Archived
    }

    [Table("documents")]
    public class Document
    {
        [Key]
        [Column("documentid")]
        public int DocumentId { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("filepath")]
        public string FilePath { get; set; }

        [Column("uploaddate")]
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        [Column("uploadedby")]
        public int UploadedBy { get; set; }

        [Column("language")]
        public string Language { get; set; } = "Vietnamese";

        [Column("type")]
        public string Type { get; set; } = "Policy";

        [Column("version")]
        public int Version { get; set; } = 1;

        // 🧩 Sử dụng enum để map đúng với PostgreSQL enum document_status
        [Column("status", TypeName = "document_status")]
        public DocumentStatus Status { get; set; } = DocumentStatus.Draft;

        [Column("jsoncontent", TypeName = "jsonb")]
        public string JsonContent { get; set; }

    }
}
