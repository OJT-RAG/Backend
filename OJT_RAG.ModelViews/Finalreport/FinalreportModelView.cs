namespace OJT_RAG.ModelView.FinalreportModelView
{
    public class FinalreportModelView
    {
        public long FinalreportId { get; set; }
        public long? UserId { get; set; }
        public long? JobPositionId { get; set; }
        public long? SemesterId { get; set; }
        public string? StudentReportFile { get; set; }
        public string? StudentReportText { get; set; }
        public string? CompanyFeedback { get; set; }
        public int? CompanyRating { get; set; }
        public string? CompanyEvaluator { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public DateTime? EvaluatedAt { get; set; }
    }
}
