namespace OJT_RAG.DTOs.SemesterCompanyDTO
{
    public class UpdateSemesterCompanyDTO
    {
        public long SemesterCompanyId { get; set; }
        public long? SemesterId { get; set; }
        public long? CompanyId { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }
}
