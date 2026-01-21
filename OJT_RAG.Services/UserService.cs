using OJT_RAG.DTOs.UserDTO;
using OJT_RAG.ModelView.UserModelView;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Enums;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.DTOs.User;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly GoogleDriveService _drive;

        public UserService(IUserRepository repo, GoogleDriveService drive)
        {
            _repo = repo;
            _drive = drive;
        }

        // ================= GET =================
        public async Task<List<UserModelView>> GetAll()
        {
            var data = await _repo.GetAllAsync();
            return data.Select(Map).ToList();
        }

        public async Task<UserModelView?> GetById(long id)
        {
            var u = await _repo.GetByIdAsync(id);
            return u == null ? null : Map(u);
        }

        // ================= CREATE =================
        public async Task<bool> Create(CreateUserDTO dto)
        {
            string? avatarUrl = null;
            string? cvUrl = null;

            var root = await _drive.GetOrCreateFolderAsync("OJT_RAG");

            if (dto.AvatarUrl != null)
            {
                var avatarFolder = await _drive.GetOrCreateFolderAsync("USER_AVATAR", root);
                avatarUrl = await _drive.UploadFileAsync(dto.AvatarUrl, avatarFolder);
            }

            if (dto.CvUrl != null)
            {
                var cvFolder = await _drive.GetOrCreateFolderAsync("USER_CV", root);
                var userFolder = await _drive.GetOrCreateFolderAsync(dto.Email.Replace("@", "_"), cvFolder);
                cvUrl = await _drive.UploadFileAsync(dto.CvUrl, userFolder);
            }

            var user = new User
            {
                MajorId = dto.MajorId,
                CompanyId = dto.CompanyId,
                Email = dto.Email,
                Password = dto.Password,
                Fullname = dto.Fullname,
                StudentCode = dto.StudentCode,
                Dob = dto.Dob,
                Phone = dto.Phone,
                AvatarUrl = avatarUrl,
                CvUrl = cvUrl,
                Role = "Student",
                CreateAt = DateTime.UtcNow.ToLocalTime()
            };

            await _repo.AddAsync(user);
            return true;
        }

        // ================= UPDATE =================
        public async Task<bool> Update(UpdateUserDTO dto)
{
    var u = await _repo.GetByIdAsync(dto.UserId);
    if (u == null) return false;

    var root = await _drive.GetOrCreateFolderAsync("OJT_RAG");

    if (dto.AvatarUrl != null)
    {
        var avatarFolder = await _drive.GetOrCreateFolderAsync("USER_AVATAR", root);
        u.AvatarUrl = await _drive.UploadFileAsync(dto.AvatarUrl, avatarFolder);
    }

    if (dto.CvUrl != null)
    {
        var cvFolder = await _drive.GetOrCreateFolderAsync("USER_CV", root);
        u.CvUrl = await _drive.UploadFileAsync(dto.CvUrl, cvFolder);
    }

    if (dto.MajorId.HasValue)
        u.MajorId = dto.MajorId.Value;

    if (dto.CompanyId.HasValue)
        u.CompanyId = dto.CompanyId.Value;

    if (!string.IsNullOrEmpty(dto.Fullname))
        u.Fullname = dto.Fullname;

    if (!string.IsNullOrEmpty(dto.StudentCode))
        u.StudentCode = dto.StudentCode;

    if (dto.Dob.HasValue)
        u.Dob = dto.Dob.Value;

    if (!string.IsNullOrEmpty(dto.Phone))
        u.Phone = dto.Phone;

    if (!string.IsNullOrEmpty(dto.Password))
        u.Password = dto.Password;

            if (u.CreateAt.HasValue)
            {
                u.CreateAt = DateTime.SpecifyKind(
                    u.CreateAt.Value,
                    DateTimeKind.Unspecified
                );
            }

            u.UpdateAt = DateTime.SpecifyKind(
                DateTime.UtcNow,
                DateTimeKind.Unspecified
            );



            await _repo.UpdateAsync(u);
    return true;
}

        public async Task<bool> UpdateStatus(UpdateUserStatusDTO dto)
        {
            return await _repo.UpdateAccountStatusAsync(
                dto.UserId,
                dto.AccountStatus
            );
        }


        // ================= DELETE =================
        public async Task<bool> Delete(long id)
        {
            await _repo.DeleteAsync(id);
            return true;
        }

        // ================= LOGIN =================
        public async Task<UserModelView> Login(string email, string password)
        {
            var u = await _repo.GetByEmailAsync(email)
                ?? throw new UnauthorizedAccessException("Email or password is incorrect");

            if (u.AccountStatus == AccountStatusEnum.inactive)
                throw new UnauthorizedAccessException("Account is inactive");

            if (u.Password != password)
                throw new UnauthorizedAccessException("Email or password is incorrect");

            return Map(u);
        }




        private static UserModelView Map(User u)
        {
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
    }
}
