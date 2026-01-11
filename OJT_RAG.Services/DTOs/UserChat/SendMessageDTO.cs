namespace OJT_RAG.DTOs.ChatDTO
{
    public class SendMessageDTO
    {
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public string Content { get; set; } = null!;
    }
}
