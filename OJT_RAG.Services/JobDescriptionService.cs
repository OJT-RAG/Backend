using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Services.DTOs.JobDescription;
using OJT_RAG.Services.Interfaces;
using System;

namespace OJT_RAG.Services.Implementations
{
    public class JobDescriptionService : IJobDescriptionService
    {
        private readonly OJTRAGContext _context;

        public JobDescriptionService(OJTRAGContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobDescription>> GetAllAsync()
        {
            return await _context.JobDescriptions.ToListAsync();
        }

        public async Task<JobDescription?> GetByIdAsync(long id)
        {
            return await _context.JobDescriptions.FindAsync(id);
        }

        public async Task<JobDescription> CreateAsync(CreateJobDescriptionDTO dto)
        {
            var entity = new JobDescription
            {
                JobPositionId = dto.JobPositionId,
                JobDescription1 = dto.JobDescription,
                HireQuantity = dto.HireQuantity,
                AppliedQuantity = dto.AppliedQuantity
            };

            _context.JobDescriptions.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<JobDescription?> UpdateAsync(UpdateJobDescriptionDTO dto)
        {
            var entity = await _context.JobDescriptions.FindAsync(dto.JobDescriptionId);
            if (entity == null) return null;

            entity.JobPositionId = dto.JobPositionId;
            entity.JobDescription1 = dto.JobDescription;
            entity.HireQuantity = dto.HireQuantity;
            entity.AppliedQuantity = dto.AppliedQuantity;

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.JobDescriptions.FindAsync(id);
            if (entity == null) return false;

            _context.JobDescriptions.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
