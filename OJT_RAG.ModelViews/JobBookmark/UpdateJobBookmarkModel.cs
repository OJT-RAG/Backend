namespace OJT_RAG.Model.JobBookmarkModel
{
    public class UpdateJobBookmarkModel
    {
        public long JobBookmarkId { get; set; }
        public long? UserId { get; set; }
        public long? JobPositionId { get; set; }
    }
}
