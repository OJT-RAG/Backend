using Google.Apis.Auth;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Auth;

public class GoogleAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly JwtService _jwtService;

    public GoogleAuthService(
        IUserRepository userRepo,
        JwtService jwtService)
    {
        _userRepo = userRepo;
        _jwtService = jwtService;
    }

    public async Task<string> LoginWithGoogleAsync(string idToken)
    {
        // ✅ Verify token với Google
        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

        var email = payload.Email;
        var name = payload.Name;

        // ✅ Tìm user theo email
        var user = await _userRepo.GetByEmailAsync(email);

        // ❗ Nếu chưa tồn tại → tạo mới
        if (user == null)
        {
            user = new User
            {
                Email = email,
                Fullname = name,
                CreateAt = DateTime.UtcNow.ToLocalTime()
            };

            await _userRepo.AddAsync(user);
        }

        // ✅ Tạo JWT nội bộ
        return _jwtService.GenerateToken(user.UserId, user.Email);
    }
}
