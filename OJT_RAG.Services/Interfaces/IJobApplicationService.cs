using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Services.DTOs.JobApplication;

namespace OJT_RAG.Services.Interfaces
{
    public interface IJobApplicationService
    {
        Task<IEnumerable<JobApplication>> GetAllAsync();
        Task<JobApplication?> GetByIdAsync(long id);
        Task<JobApplication> CreateAsync(CreateJobApplicationDTO dto);
        Task<JobApplication?> UpdateStatusAsync(UpdateJobApplicationStatusDTO dto);
        Task<bool> DeleteByIdAsync(long id);
    }
}
