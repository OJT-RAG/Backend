namespace OJT_RAG.ModelView.SemesterCompanyModelView
{
    public class SemesterCompanyModelView
    {
        public long SemesterCompanyId { get; set; }
        public long? SemesterId { get; set; }
        public long? CompanyId { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }
}
