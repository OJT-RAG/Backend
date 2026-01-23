using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using OJT_RAG.ModelView.UserModelView;
using OJT_RAG.Repositories;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Services.Auth
{
    public class GoogleAuthService
    {
        private readonly OJTRAGContext _context;
        private readonly JwtService _jwtService;

        public GoogleAuthService(OJTRAGContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<(string Token, UserModelView UserInfo)> LoginWithGoogleAsync(string idToken)
        {
            var clientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");

            if (string.IsNullOrWhiteSpace(clientId))
                throw new Exception("GOOGLE_CLIENT_ID not found");

            var payload = await GoogleJsonWebSignature.ValidateAsync(
                idToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { clientId }
                }
            );

            // Tìm user theo email
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == payload.Email);

            // Nếu chưa có thì tạo mới (Lần đầu login)
            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    Fullname = payload.Name,
                    AvatarUrl = payload.Picture,
                    Role = "Student", // Mặc định role
                    CreateAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // Tạo JWT Token
            var token = _jwtService.GenerateToken(user.UserId, user.Email!);

            // Map dữ liệu sang UserModelView để trả về cho Client
            var userView = new UserModelView
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

            return (token, userView);
        }
    }
}
