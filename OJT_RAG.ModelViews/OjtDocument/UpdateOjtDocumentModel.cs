namespace OJT_RAG.Model.OjtDocumentModel
{
    public class UpdateOjtDocumentModel
    {
        public string? OjtdocumentId { get; set; }
        public string? Title { get; set; }
        public string? SemesterId { get; set; }
        public bool? IsGeneral { get; set; }
        public string? UploadedBy { get; set; }
    }

}
