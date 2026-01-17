using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OJT_RAG.Services.DTOs.JobApplication
{
    public class CreateJobApplicationDTO
    {
        public long UserId { get; set; }
        public long JobPositionId { get; set; }
    }
}
