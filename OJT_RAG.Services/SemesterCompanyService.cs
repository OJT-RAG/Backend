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

        public SemesterCompanyService(ISemesterCompanyRepository repo)
        {
            _repo = repo;
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
            var entity = new SemesterCompany
            {
                SemesterId = dto.SemesterId,
                CompanyId = dto.CompanyId,
                ApprovedAt = null
            };

            await _repo.AddAsync(entity);
            return true;
        }

        public async Task<bool> Update(UpdateSemesterCompanyDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.SemesterCompanyId);
            if (entity == null) return false;

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
