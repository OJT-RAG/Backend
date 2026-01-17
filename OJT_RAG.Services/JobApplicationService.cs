using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.DTOs.JobApplication;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IJobApplicationRepository _repo;

        public JobApplicationService(IJobApplicationRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<JobApplication>> GetAllAsync()
        {
            return await _repo.GetAll();
        }

        public async Task<JobApplication?> GetByIdAsync(long id)
        {
            return await _repo.GetById(id);
        }

        public async Task<JobApplication> CreateAsync(CreateJobApplicationDTO dto)
        {
            // Chặn apply trùng
            var exist = await _repo.GetByUserAndPosition(dto.UserId, dto.JobPositionId);
            if (exist != null)
                throw new Exception("User đã apply vị trí này rồi");

            var entity = new JobApplication
            {
                UserId = dto.UserId,
                JobPositionId = dto.JobPositionId,
                Status = "pending",
                AppliedAt = DateTime.UtcNow.ToLocalTime(),
                CreateAt = DateTime.UtcNow.ToLocalTime(),
                UpdateAt = DateTime.UtcNow.ToLocalTime(),
                IsRandomFallback = false
            };

            return await _repo.Add(entity);
        }

        public async Task<JobApplication?> UpdateStatusAsync(UpdateJobApplicationStatusDTO dto)
        {
            var entity = await _repo.GetById(dto.JobApplicationId);
            if (entity == null) return null;

            entity.Status = dto.Status;
            entity.RejectedReason = dto.RejectedReason;
            entity.CompanyDecisionAt = DateTime.UtcNow.ToLocalTime();
            entity.UpdateAt = DateTime.UtcNow.ToLocalTime();

            return await _repo.Update(entity);
        }
    }
}
