namespace OJT_RAG.DTOs.UserDTO
{
    public class UpdateUserDTO
    {
        public long UserId { get; set; }
        public long? MajorId { get; set; }
        public long? CompanyId { get; set; }
        public string? Fullname { get; set; }
        public string? StudentCode { get; set; }
        public DateOnly? Dob { get; set; }
        public string? Phone { get; set; }
        public string? AvatarUrl { get; set; }
        public string? CvUrl { get; set; }
        public string? Password { get; set; }
    }
}
