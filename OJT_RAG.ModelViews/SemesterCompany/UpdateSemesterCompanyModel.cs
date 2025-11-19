namespace OJT_RAG.Model.SemesterCompanyModel
{
    public class UpdateSemesterCompanyModel
    {
        public long SemesterCompanyId { get; set; }
        public long? SemesterId { get; set; }
        public long? CompanyId { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }
}
