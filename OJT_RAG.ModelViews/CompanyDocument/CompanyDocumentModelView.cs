namespace OJT_RAG.ModelViews.CompanyDocument
{
    public class CompanyDocumentModelView
    {
        public long CompanyDocumentId { get; set; }
        public long? SemesterCompanyId { get; set; }

        public string? Title { get; set; }
        public string? FileUrl { get; set; }
        public long? UploadedBy { get; set; }
        public bool? IsPublic { get; set; }
    }
}
