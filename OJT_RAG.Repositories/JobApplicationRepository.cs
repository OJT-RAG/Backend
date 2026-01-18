using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Repositories
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly OJTRAGContext _context;

        public JobApplicationRepository(OJTRAGContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobApplication>> GetAll()
        {
            return await _context.JobApplications.ToListAsync();
        }

        public async Task<JobApplication?> GetById(long id)
        {
            return await _context.JobApplications.FindAsync(id);
        }

        public async Task<JobApplication?> GetByUserAndPosition(long userId, long jobPositionId)
        {
            return await _context.JobApplications.FirstOrDefaultAsync(x =>
                x.UserId == userId && x.JobPositionId == jobPositionId);
        }

        public async Task<JobApplication> Add(JobApplication entity)
        {
            _context.JobApplications.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<JobApplication> Update(JobApplication entity)
        {
            try
            {
                _context.JobApplications.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task Delete(JobApplication entity)
        {
            _context.JobApplications.Remove(entity);
            await _context.SaveChangesAsync();
        }

    }
}
