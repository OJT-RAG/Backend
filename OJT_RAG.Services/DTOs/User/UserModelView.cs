using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.ModelView.UserModelView
{
    public class UserModelView
    {
        public long UserId { get; set; }
        public long? MajorId { get; set; }
        public long? CompanyId { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Fullname { get; set; }
        public string? StudentCode { get; set; }
        public DateOnly? Dob { get; set; }
        public string? Phone { get; set; }
        public string? AvatarUrl { get; set; }
        public string? CvUrl { get; set; }
        public DateTime? CreateAt { get; set; }
    }
}
