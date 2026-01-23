namespace OJT_RAG.ModelView.OjtDocumentModelView
{
    public class OjtDocumentModelView
    {
        public long OjtdocumentId { get; set; }
        public string? Title { get; set; }
        public string? FileUrl { get; set; }
        public long? SemesterId { get; set; }
        public bool? IsGeneral { get; set; }
        public long? UploadedBy { get; set; }
        public string? UploaderName { get; set; } // Thêm cái này
        public DateTime? UploadedAt { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
