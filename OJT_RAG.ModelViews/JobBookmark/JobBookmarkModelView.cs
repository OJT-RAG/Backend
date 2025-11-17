namespace OJT_RAG.ModelView.JobBookmarkModelView
{
    public class JobBookmarkModelView
    {
        public long JobBookmarkId { get; set; }
        public long? UserId { get; set; }
        public long? JobPositionId { get; set; }
        public DateTime? CreateAt { get; set; }
    }
}
