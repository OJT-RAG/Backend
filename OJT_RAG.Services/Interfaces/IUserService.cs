using OJT_RAG.DTOs.UserDTO;
using OJT_RAG.ModelView.UserModelView;

namespace OJT_RAG.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserModelView>> GetAll();
        Task<UserModelView?> GetById(long id);
        Task<bool> Create(CreateUserDTO dto);
        Task<bool> Update(UpdateUserDTO dto);
        Task<bool> Delete(long id);
    }
}
