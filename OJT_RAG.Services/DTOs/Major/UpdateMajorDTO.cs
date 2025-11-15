namespace OJT_RAG.Services.DTOs.Major
{
    public class UpdateMajorDTO
    {
        public long MajorId { get; set; }
        public string? MajorTitle { get; set; }
        public string? MajorCode { get; set; }
        public string? Description { get; set; }
    }
}
