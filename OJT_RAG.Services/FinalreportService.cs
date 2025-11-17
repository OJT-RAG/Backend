using OJT_RAG.DTOs.FinalreportDTO;
using OJT_RAG.ModelView.FinalreportModelView;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services
{
    public class FinalreportService : IFinalreportService
    {
        private readonly IFinalreportRepository _repo;

        public FinalreportService(IFinalreportRepository repo)
        {
            _repo = repo;
        }

        private FinalreportModelView Map(Finalreport x)
        {
            return new FinalreportModelView
            {
                FinalreportId = x.FinalreportId,
                UserId = x.UserId,
                JobPositionId = x.JobPositionId,
                SemesterId = x.SemesterId,
                StudentReportFile = x.StudentReportFile,
                StudentReportText = x.StudentReportText,
                CompanyFeedback = x.CompanyFeedback,
                CompanyRating = x.CompanyRating,
                CompanyEvaluator = x.CompanyEvaluator,
                SubmittedAt = x.SubmittedAt,
                EvaluatedAt = x.EvaluatedAt
            };
        }

        public async Task<IEnumerable<FinalreportModelView>> GetAll()
        {
            return (await _repo.GetAllAsync()).Select(Map);
        }

        public async Task<FinalreportModelView?> GetById(long id)
        {
            var data = await _repo.GetByIdAsync(id);
            return data == null ? null : Map(data);
        }

        public async Task<IEnumerable<FinalreportModelView>> GetByUser(long userId)
        {
            return (await _repo.GetByUserIdAsync(userId)).Select(Map);
        }

        public async Task<IEnumerable<FinalreportModelView>> GetBySemester(long semesterId)
        {
            return (await _repo.GetBySemesterIdAsync(semesterId)).Select(Map);
        }

        public async Task<IEnumerable<FinalreportModelView>> GetByJob(long jobPositionId)
        {
            return (await _repo.GetByJobPositionIdAsync(jobPositionId)).Select(Map);
        }

        public async Task<bool> Create(CreateFinalreportDTO dto)
        {
            var entity = new Finalreport
            {
                UserId = dto.UserId,
                JobPositionId = dto.JobPositionId,
                SemesterId = dto.SemesterId,
                StudentReportFile = dto.StudentReportFile,
                StudentReportText = dto.StudentReportText,
                CompanyFeedback = dto.CompanyFeedback,
                CompanyRating = dto.CompanyRating,
                CompanyEvaluator = dto.CompanyEvaluator,
                SubmittedAt = DateTime.UtcNow.ToLocalTime()
            };

            await _repo.AddAsync(entity);
            return true;
        }

        public async Task<bool> Update(UpdateFinalreportDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.FinalreportId);
            if (entity == null) return false;

            entity.UserId = dto.UserId;
            entity.JobPositionId = dto.JobPositionId;
            entity.SemesterId = dto.SemesterId;
            entity.StudentReportFile = dto.StudentReportFile;
            entity.StudentReportText = dto.StudentReportText;
            entity.CompanyFeedback = dto.CompanyFeedback;
            entity.CompanyRating = dto.CompanyRating;
            entity.CompanyEvaluator = dto.CompanyEvaluator;

            entity.EvaluatedAt = DateTime.UtcNow.ToLocalTime();

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> Delete(long id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
