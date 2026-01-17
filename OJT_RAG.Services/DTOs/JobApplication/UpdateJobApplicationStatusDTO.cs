using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OJT_RAG.Services.DTOs.JobApplication
{
    public class UpdateJobApplicationStatusDTO
    {
        public long JobApplicationId { get; set; }
        public string Status { get; set; }   // approved / rejected
        public string? RejectedReason { get; set; }
    }

}
