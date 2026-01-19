using OJT_RAG.DTOs.JobPositionDTO;
using OJT_RAG.ModelView.JobPositionModelView;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OJT_RAG.Services
{
    public class JobPositionService : IJobPositionService
    {
        private readonly IJobPositionRepository _jobPositionRepo;
        private readonly ISemesterCompanyRepository _semesterCompanyRepo;

        public JobPositionService(
            IJobPositionRepository jobPositionRepo,
            ISemesterCompanyRepository semesterCompanyRepo)
        {
            _jobPositionRepo = jobPositionRepo;
            _semesterCompanyRepo = semesterCompanyRepo;
        }

        public async Task<IEnumerable<JobPositionModelView>> GetAll()
        {
            var entities = await _jobPositionRepo.GetAllAsync();
            return entities.Select(x => new JobPositionModelView
            {
                JobPositionId = x.JobPositionId,
                MajorId = x.MajorId,
                SemesterId = x.SemesterId,
                SemesterCompanyId = x.SemesterCompanyId ?? 0,
                JobTitle = x.JobTitle ?? string.Empty,
                Requirements = x.Requirements,
                Benefit = x.Benefit,
                Location = x.Location,
                SalaryRange = x.SalaryRange,
                IsActive = x.IsActive ?? false,
                CreateAt = x.CreateAt ?? DateTime.UtcNow.ToLocalTime(),
                UpdateAt = x.UpdateAt ?? DateTime.UtcNow.ToLocalTime()
            }).ToList();
        }

        public async Task<JobPositionModelView?> GetById(long id)
        {
            var x = await _jobPositionRepo.GetByIdAsync(id);
            if (x == null) return null;

            return new JobPositionModelView
            {
                JobPositionId = x.JobPositionId,
                MajorId = x.MajorId,
                SemesterId = x.SemesterId,
                SemesterCompanyId = x.SemesterCompanyId ?? 0,
                JobTitle = x.JobTitle ?? string.Empty,
                Requirements = x.Requirements,
                Benefit = x.Benefit,
                Location = x.Location,
                SalaryRange = x.SalaryRange,
                IsActive = x.IsActive ?? false,
                CreateAt = x.CreateAt ?? DateTime.UtcNow.ToLocalTime(),
                UpdateAt = x.UpdateAt ?? DateTime.UtcNow.ToLocalTime()
            };
        }

        public async Task<bool> Create(CreateJobPositionDTO dto)
        {
            // Validation bắt buộc (dto.SemesterCompanyId là long non-nullable)
            if (dto.SemesterCompanyId <= 0)
            {
                throw new ArgumentException("SemesterCompanyId là bắt buộc và phải lớn hơn 0.");
            }

            // Kiểm tra tồn tại
            var semesterCompany = await _semesterCompanyRepo.GetByIdAsync(dto.SemesterCompanyId);
            if (semesterCompany == null)
            {
                throw new ArgumentException($"Kỳ thực tập của công ty (ID: {dto.SemesterCompanyId}) không tồn tại.");
            }

            var entity = new JobPosition
            {
                MajorId = dto.MajorId,
                SemesterId = dto.SemesterId,
                SemesterCompanyId = dto.SemesterCompanyId,
                JobTitle = dto.JobTitle ?? throw new ArgumentNullException(nameof(dto.JobTitle), "JobTitle là bắt buộc"),
                Requirements = dto.Requirements,
                Benefit = dto.Benefit,
                Location = dto.Location,
                SalaryRange = dto.SalaryRange,
                IsActive = dto.IsActive,  // Không cần ?? vì DTO dùng bool
                CreateAt = DateTime.UtcNow,  // An toàn với timestamp without time zone
                UpdateAt = DateTime.UtcNow
            };

            await _jobPositionRepo.AddAsync(entity);
            return true;
        }

        public async Task<bool> Update(UpdateJobPositionDTO dto)
        {
            var entity = await _jobPositionRepo.GetByIdAsync(dto.JobPositionId);
            if (entity == null) return false;

            // Validate nếu update SemesterCompanyId
            if (dto.SemesterCompanyId.HasValue && dto.SemesterCompanyId.Value != entity.SemesterCompanyId)
            {
                var newSc = await _semesterCompanyRepo.GetByIdAsync(dto.SemesterCompanyId.Value);
                if (newSc == null)
                    throw new ArgumentException("Kỳ thực tập của công ty mới không tồn tại.");
            }

            entity.MajorId = dto.MajorId;
            entity.SemesterId = dto.SemesterId;
            entity.SemesterCompanyId = dto.SemesterCompanyId ?? entity.SemesterCompanyId;
            entity.JobTitle = dto.JobTitle ?? entity.JobTitle;
            entity.Requirements = dto.Requirements ?? entity.Requirements;
            entity.Benefit = dto.Benefit ?? entity.Benefit;
            entity.Location = dto.Location ?? entity.Location;
            entity.SalaryRange = dto.SalaryRange ?? entity.SalaryRange;
            entity.IsActive = dto.IsActive ?? entity.IsActive ?? false;
            entity.UpdateAt = DateTime.UtcNow;

            await _jobPositionRepo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> Delete(long id)
        {
            return await _jobPositionRepo.DeleteAsync(id);
        }

        // Bonus: Lấy job theo công ty + kỳ (filter in-memory)
        public async Task<IEnumerable<JobPositionModelView>> GetByCompanyAndSemester(long companyId, long semesterId)
        {
            var allSc = await _semesterCompanyRepo.GetAllAsync();
            var matchingSc = allSc.FirstOrDefault(sc => sc.CompanyId == companyId && sc.SemesterId == semesterId);
            if (matchingSc == null) return Enumerable.Empty<JobPositionModelView>();

            var allJobs = await _jobPositionRepo.GetAllAsync();
            var jobs = allJobs.Where(j => j.SemesterCompanyId == matchingSc.SemesterCompanyId);

            return jobs.Select(j => new JobPositionModelView
            {
                JobPositionId = j.JobPositionId,
                MajorId = j.MajorId,
                SemesterId = j.SemesterId,
                SemesterCompanyId = j.SemesterCompanyId ?? 0,
                JobTitle = j.JobTitle ?? string.Empty,
                Requirements = j.Requirements,
                Benefit = j.Benefit,
                Location = j.Location,
                SalaryRange = j.SalaryRange,
                IsActive = j.IsActive ?? false,
                CreateAt = j.CreateAt ?? DateTime.UtcNow.ToLocalTime(),
                UpdateAt = j.UpdateAt ?? DateTime.UtcNow.ToLocalTime()
            }).ToList();
        }
        public async Task<bool> HasJobApplication(long jobPositionId)
        {
            return await _jobPositionRepo.HasJobApplicationAsync(jobPositionId);
        }

    }
}