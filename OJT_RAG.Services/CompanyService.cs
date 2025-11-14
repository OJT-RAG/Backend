using OJT_RAG.ModelViews.Company;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repo;

        public CompanyService(ICompanyRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<CompanyViewModel>> GetAllAsync()
        {
            var data = await _repo.GetAll();

            return data.Select(c => new CompanyViewModel
            {
                Company_ID = c.CompanyId,
                MajorID = c.MajorId,
                Name = c.Name,
                Tax_Code = c.TaxCode,
                Address = c.Address,
                Website = c.Website,
                Contact_Email = c.ContactEmail,
                Phone = c.Phone,
                Logo_URL = c.LogoUrl,
                Is_Verified = c.IsVerified
            });
        }

        public async Task<CompanyViewModel?> GetByIdAsync(long id)
        {
            var c = await _repo.GetById(id);
            if (c == null) return null;

            return new CompanyViewModel
            {
                Company_ID = c.CompanyId,
                MajorID = c.MajorId,
                Name = c.Name,
                Tax_Code = c.TaxCode,
                Address = c.Address,
                Website = c.Website,
                Contact_Email = c.ContactEmail,
                Phone = c.Phone,
                Logo_URL = c.LogoUrl,
                Is_Verified = c.IsVerified
            };
        }

        public async Task<CompanyViewModel> CreateAsync(CompanyCreateModel model)
        {
            var newId = await _repo.GetNextId();

            var entity = new Company
            {
                CompanyId = newId,
                MajorId = model.MajorID,
                Name = model.Name,
                TaxCode = model.Tax_Code,
                Address = model.Address,
                Website = model.Website,
                ContactEmail = model.Contact_Email,
                Phone = model.Phone,
                LogoUrl = model.Logo_URL,
                IsVerified = model.Is_Verified,

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _repo.Add(entity);

            return new CompanyViewModel
            {
                Company_ID = entity.CompanyId,
                MajorID = entity.MajorId,
                Name = entity.Name,
                Tax_Code = entity.TaxCode,
                Address = entity.Address,
                Website = entity.Website,
                Contact_Email = entity.ContactEmail,
                Phone = entity.Phone,
                Logo_URL = entity.LogoUrl,
                Is_Verified = entity.IsVerified
            };
        }

        public async Task<CompanyViewModel?> UpdateAsync(CompanyUpdateModel model)
        {
            var entity = await _repo.GetById(model.Company_ID);
            if (entity == null) return null;

            entity.MajorId = model.MajorID ?? entity.MajorId;
            entity.Name = model.Name ?? entity.Name;
            entity.TaxCode = model.Tax_Code ?? entity.TaxCode;
            entity.Address = model.Address ?? entity.Address;
            entity.Website = model.Website ?? entity.Website;
            entity.ContactEmail = model.Contact_Email ?? entity.ContactEmail;
            entity.Phone = model.Phone ?? entity.Phone;
            entity.LogoUrl = model.Logo_URL ?? entity.LogoUrl;
            entity.IsVerified = model.Is_Verified;

            entity.UpdatedAt = DateTime.UtcNow;

            await _repo.Update(entity);

            return new CompanyViewModel
            {
                Company_ID = entity.CompanyId,
                MajorID = entity.MajorId,
                Name = entity.Name,
                Tax_Code = entity.TaxCode,
                Address = entity.Address,
                Website = entity.Website,
                Contact_Email = entity.ContactEmail,
                Phone = entity.Phone,
                Logo_URL = entity.LogoUrl,
                Is_Verified = entity.IsVerified
            };
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _repo.GetById(id);
            if (entity == null) return false;

            await _repo.Delete(id);
            return true;
        }
    }
}
