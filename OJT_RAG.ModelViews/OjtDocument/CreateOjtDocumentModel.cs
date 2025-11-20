namespace OJT_RAG.Model.OjtDocumentModel
{
    public class CreateOjtDocumentModel
    {
        public string? Title { get; set; }
        public long? SemesterId { get; set; }
        public bool? IsGeneral { get; set; }
        public long? UploadedBy { get; set; }
    }
}
