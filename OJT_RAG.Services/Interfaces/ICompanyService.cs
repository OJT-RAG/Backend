using OJT_RAG.ModelViews.Company;
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyViewModel>> GetAllAsync();
        Task<CompanyViewModel?> GetByIdAsync(long id);
        Task<CompanyViewModel> CreateAsync(CompanyCreateModel model);
        Task<CompanyViewModel?> UpdateAsync(CompanyUpdateModel model);
        Task<bool> DeleteAsync(long id);
    }
}