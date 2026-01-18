using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IJobApplicationRepository
    {
        Task<IEnumerable<JobApplication>> GetAll();
        Task<JobApplication?> GetById(long id);
        Task<JobApplication?> GetByUserAndPosition(long userId, long jobPositionId);
        Task<JobApplication> Add(JobApplication entity);
        Task<JobApplication> Update(JobApplication entity);
        Task Delete(JobApplication entity);

    }
}
