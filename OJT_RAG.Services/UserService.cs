using OJT_RAG.DTOs.UserDTO;
using OJT_RAG.ModelView.UserModelView;
using OJT_RAG.Repositories;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;
namespace OJT_RAG.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<UserModelView>> GetAll()
        {
            var data = await _repo.GetAllAsync();

            return data.Select(u => new UserModelView
            {
                UserId = u.UserId,
                MajorId = u.MajorId,
                CompanyId = u.CompanyId,
                Email = u.Email,
                Role = u.Role,
                Fullname = u.Fullname,
                StudentCode = u.StudentCode,
                Dob = u.Dob,
                Phone = u.Phone,
                AvatarUrl = u.AvatarUrl,
                CvUrl = u.CvUrl,
                CreateAt = u.CreateAt
            }).ToList();
        }

        public async Task<UserModelView?> GetById(long id)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) return null;

            return new UserModelView
            {
                UserId = u.UserId,
                MajorId = u.MajorId,
                CompanyId = u.CompanyId,
                Email = u.Email,
                Role = u.Role,
                Fullname = u.Fullname,
                StudentCode = u.StudentCode,
                Dob = u.Dob,
                Phone = u.Phone,
                AvatarUrl = u.AvatarUrl,
                CvUrl = u.CvUrl,
                CreateAt = u.CreateAt
            };
        }

        public async Task<bool> Create(CreateUserDTO dto)
        {
            var user = new User
            {
                MajorId = dto.MajorId,
                CompanyId = dto.CompanyId,
                Email = dto.Email,
                Password = dto.Password,
                Role = dto.Role,
                Fullname = dto.Fullname,
                StudentCode = dto.StudentCode,
                Dob = dto.Dob,
                Phone = dto.Phone,
                AvatarUrl = dto.AvatarUrl,
                CvUrl = dto.CvUrl,
                CreateAt = DateTime.UtcNow.ToLocalTime(),
                UpdateAt = DateTime.UtcNow.ToLocalTime()
            };

            await _repo.AddAsync(user);
            return true;
        }

        public async Task<bool> Update(UpdateUserDTO dto)
        {
            var u = await _repo.GetByIdAsync(dto.UserId);
            if (u == null) return false;

            u.MajorId = dto.MajorId;
            u.CompanyId = dto.CompanyId;
            u.Role = dto.Role;
            u.Fullname = dto.Fullname;
            u.StudentCode = dto.StudentCode;
            u.Dob = dto.Dob;
            u.Phone = dto.Phone;
            u.AvatarUrl = dto.AvatarUrl;
            u.CvUrl = dto.CvUrl;

            if (!string.IsNullOrEmpty(dto.Password))
                u.Password = dto.Password;

            u.UpdateAt = DateTime.UtcNow.ToLocalTime();

            await _repo.UpdateAsync(u);
            return true;
        }

        public async Task<bool> Delete(long id)
        {
            await _repo.DeleteAsync(id);
            return true;
        }

        public async Task<UserModelView?> Login(string email, string password)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null) return null;

            // NOTE: Currently comparing plain text passwords.
            // For production, replace this with proper password hashing & verification.
            if (user.Password != password) return null;

            return new UserModelView
            {
                UserId = user.UserId,
                MajorId = user.MajorId,
                CompanyId = user.CompanyId,
                Email = user.Email,
                Role = user.Role,
                Fullname = user.Fullname,
                StudentCode = user.StudentCode,
                Dob = user.Dob,
                Phone = user.Phone,
                AvatarUrl = user.AvatarUrl,
                CvUrl = user.CvUrl,
                CreateAt = user.CreateAt
            };
        }
    }
}
