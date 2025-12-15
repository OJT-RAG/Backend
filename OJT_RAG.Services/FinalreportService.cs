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
        private readonly GoogleDriveService _drive;

        public FinalreportService(IFinalreportRepository repo, GoogleDriveService drive)
        {
            _repo = repo;
            _drive = drive;
        }

        // Map Finalreport -> ModelView
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

        // GET ALL
        public async Task<IEnumerable<FinalreportModelView>> GetAll()
            => (await _repo.GetAllAsync()).Select(Map);

        // GET BY ID
        public async Task<FinalreportModelView?> GetById(long id)
            => (await _repo.GetByIdAsync(id)) is Finalreport x ? Map(x) : null;

        // GET BY USER
        public async Task<IEnumerable<FinalreportModelView>> GetByUser(long userId)
            => (await _repo.GetByUserIdAsync(userId)).Select(Map);

        // GET BY SEMESTER
        public async Task<IEnumerable<FinalreportModelView>> GetBySemester(long semesterId)
            => (await _repo.GetBySemesterIdAsync(semesterId)).Select(Map);

        // GET BY JOB POSITION
        public async Task<IEnumerable<FinalreportModelView>> GetByJob(long jobPositionId)
            => (await _repo.GetByJobPositionIdAsync(jobPositionId)).Select(Map);

        // CREATE FINAL REPORT
        public async Task<bool> Create(CreateFinalreportDTO dto)
        {
            string? fileUrl = null;

            if (dto.File != null)
            {
                // Root folder: OJT_RAG
                var mainRootFolder = await _drive.GetOrCreateFolderAsync("OJT_RAG");

                // Sub folder: OJT_FinalReports
                var finalReportRoot = await _drive.GetOrCreateFolderAsync("OJT_FinalReports", mainRootFolder);

                // Sub folder: user_{id}
                var userFolder = await _drive.GetOrCreateFolderAsync($"user_{dto.UserId}", finalReportRoot);

                // Upload file
                fileUrl = await _drive.UploadFileAsync(dto.File, userFolder);
            }

            var entity = new Finalreport
            {
                UserId = dto.UserId,
                JobPositionId = dto.JobPositionId,
                SemesterId = dto.SemesterId,
                StudentReportFile = fileUrl,
                StudentReportText = dto.StudentReportText,
                CompanyFeedback = dto.CompanyFeedback,
                CompanyRating = dto.CompanyRating,
                CompanyEvaluator = dto.CompanyEvaluator,
                SubmittedAt = DateTime.UtcNow.ToLocalTime()
            };

            await _repo.AddAsync(entity);
            return true;
        }

        // UPDATE FINAL REPORT
        public async Task<bool> Update(UpdateFinalreportDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.FinalreportId);
            if (entity == null) return false;

            entity.UserId = dto.UserId;
            entity.JobPositionId = dto.JobPositionId;
            entity.SemesterId = dto.SemesterId;
            entity.StudentReportText = dto.StudentReportText;
            entity.CompanyFeedback = dto.CompanyFeedback;
            entity.CompanyRating = dto.CompanyRating;
            entity.CompanyEvaluator = dto.CompanyEvaluator;

            // Nếu có upload file mới → Xóa file cũ + Upload file mới
            if (dto.File != null)
            {
                // Xóa file cũ
                if (!string.IsNullOrEmpty(entity.StudentReportFile))
                {
                    var oldFileId = _drive.ExtractFileIdFromUrl(entity.StudentReportFile);
                    if (!string.IsNullOrEmpty(oldFileId))
                    {
                        try
                        {
                            await _drive.DeleteFileByIdAsync(oldFileId);
                        }
                        catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            Console.WriteLine($"File {oldFileId} không tồn tại trên Drive, bỏ qua.");
                        }
                        catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            Console.WriteLine($"Không có quyền xóa file {oldFileId} trên Drive.");
                        }
                    }
                }

                // Lấy folder gốc OJT_RAG
                var mainRootFolder = await _drive.GetOrCreateFolderAsync("OJT_RAG");

                // Folder OJT_FinalReports
                var finalReportRoot = await _drive.GetOrCreateFolderAsync("OJT_FinalReports", mainRootFolder);

                // Folder user
                var userFolder = await _drive.GetOrCreateFolderAsync($"user_{dto.UserId}", finalReportRoot);

                // Upload file mới
                entity.StudentReportFile = await _drive.UploadFileAsync(dto.File, userFolder);
            }

            entity.EvaluatedAt = DateTime.UtcNow.ToLocalTime();

            await _repo.UpdateAsync(entity);
            return true;
        }


        // DELETE FINAL REPORT
        public async Task<bool> Delete(long id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            if (!string.IsNullOrEmpty(entity.StudentReportFile))
            {
                var fileId = _drive.ExtractFileIdFromUrl(entity.StudentReportFile);
                if (!string.IsNullOrEmpty(fileId))
                {
                    try
                    {
                        await _drive.DeleteFileByIdAsync(fileId);
                    }
                    catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        Console.WriteLine($"File {fileId} không tồn tại trên Drive.");
                    }
                    catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        Console.WriteLine($"Không có quyền xóa file {fileId} trên Drive.");
                    }
                }
            }

            return await _repo.DeleteAsync(id);
        }

        public async Task<(byte[] fileBytes, string fileName, string contentType)?> Download(long id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null || string.IsNullOrEmpty(entity.StudentReportFile))
                return null;

            return await _drive.DownloadFileByUrlAsync(entity.StudentReportFile);
        }
    }
}
