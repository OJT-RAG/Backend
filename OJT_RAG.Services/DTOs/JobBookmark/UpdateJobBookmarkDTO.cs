namespace OJT_RAG.DTOs.JobBookmarkDTO
{
    public class UpdateJobBookmarkDTO
    {
        public long JobBookmarkId { get; set; }
        public long? UserId { get; set; }
        public long? JobPositionId { get; set; }
    }
}
