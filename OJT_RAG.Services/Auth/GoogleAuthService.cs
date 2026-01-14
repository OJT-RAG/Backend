using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
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

        public async Task<string> LoginWithGoogleAsync(string idToken)
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

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == payload.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    Fullname = payload.Name,
                    AvatarUrl = payload.Picture,
                    Role = UserRole.student.ToString(),
                    CreateAt = DateTime.UtcNow.ToLocalTime(),
                    UpdateAt = DateTime.UtcNow.ToLocalTime()
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            return _jwtService.GenerateToken(user.UserId, user.Email!);
        }
    }
}
