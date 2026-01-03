using Microsoft.AspNetCore.Http;

namespace OJT_RAG.DTOs.UserDTO
{
    public class CreateUserDTO
    {
        public long? MajorId { get; set; }
        public long? CompanyId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Fullname { get; set; }
        public string? StudentCode { get; set; }
        public DateOnly? Dob { get; set; }
        public string? Phone { get; set; }
        public IFormFile? AvatarUrl { get; set; }

        // ✅ upload CV
        public IFormFile? CvUrl { get; set; }
    }
}
