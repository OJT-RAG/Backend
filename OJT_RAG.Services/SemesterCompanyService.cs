using OJT_RAG.DTOs.SemesterCompanyDTO;
using OJT_RAG.ModelView.SemesterCompanyModelView;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services
{
    public class SemesterCompanyService : ISemesterCompanyService
    {
        private readonly ISemesterCompanyRepository _repo;
        private readonly ISemesterRepository _semesterRepo;
        private readonly ICompanyRepository _companyRepo;

        public SemesterCompanyService(
            ISemesterCompanyRepository repo,
            ISemesterRepository semesterRepo,
            ICompanyRepository companyRepo)
        {
            _repo = repo;
            _semesterRepo = semesterRepo;
            _companyRepo = companyRepo;
        }

        private SemesterCompanyModelView Map(SemesterCompany x)
        {
            return new SemesterCompanyModelView
            {
                SemesterCompanyId = x.SemesterCompanyId,
                SemesterId = x.SemesterId,
                CompanyId = x.CompanyId,
                ApprovedAt = x.ApprovedAt
            };
        }

        public async Task<IEnumerable<SemesterCompanyModelView>> GetAll()
        {
            return (await _repo.GetAllAsync()).Select(Map);
        }

        public async Task<SemesterCompanyModelView?> GetById(long id)
        {
            var x = await _repo.GetByIdAsync(id);
            return x == null ? null : Map(x);
        }

        public async Task<IEnumerable<SemesterCompanyModelView>> GetBySemester(long semesterId)
        {
            return (await _repo.GetBySemesterIdAsync(semesterId)).Select(Map);
        }

        public async Task<IEnumerable<SemesterCompanyModelView>> GetByCompany(long companyId)
        {
            return (await _repo.GetByCompanyIdAsync(companyId)).Select(Map);
        }

        public async Task<bool> Create(CreateSemesterCompanyDTO dto)
        {
            if (!dto.SemesterId.HasValue)
                throw new Exception("SemesterId không được null");

            if (!dto.CompanyId.HasValue)
                throw new Exception("CompanyId không được null");

            if (!await _semesterRepo.ExistsAsync(dto.SemesterId.Value))
                throw new Exception("Semester không tồn tại");

            if (!await _companyRepo.ExistsAsync(dto.CompanyId.Value))
                throw new Exception("Company không tồn tại");

            if (await _repo.ExistsAsync(dto.SemesterId.Value, dto.CompanyId.Value))
                throw new Exception("Liên kết đã tồn tại");

            var entity = new SemesterCompany
            {
                SemesterId = dto.SemesterId.Value,
                CompanyId = dto.CompanyId.Value,
                ApprovedAt = null
            };

            await _repo.AddAsync(entity);
            return true;
        }


        public async Task<bool> Update(UpdateSemesterCompanyDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.SemesterCompanyId);
            if (entity == null) throw new Exception("Không tìm thấy liên kết");

            entity.SemesterId = dto.SemesterId;
            entity.CompanyId = dto.CompanyId;
            entity.ApprovedAt = dto.ApprovedAt;

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> Delete(long id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
