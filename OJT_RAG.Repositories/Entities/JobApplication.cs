using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OJT_RAG.Repositories.Entities
{
    [Table("job_application")]
    public class JobApplication
    {
        [Key]
        [Column("job_application_id")]
        public long JobApplicationId { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("job_position_id")]
        public long JobPositionId { get; set; }

        [Column("status")]
        public string Status { get; set; } = "pending";

        [Column("applied_at")]
        public DateTime AppliedAt { get; set; }

        [Column("company_decision_at")]
        public DateTime? CompanyDecisionAt { get; set; }

        [Column("rejected_reason")]
        public string? RejectedReason { get; set; }

        [Column("is_random_fallback")]
        public bool IsRandomFallback { get; set; }

        [Column("create_at")]
        public DateTime CreateAt { get; set; }

        [Column("update_at")]
        public DateTime UpdateAt { get; set; }
    }
}
