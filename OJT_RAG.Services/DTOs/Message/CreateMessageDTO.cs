namespace OJT_RAG.DTOs.MessageDTO
{
    public class CreateMessageDTO
    {
        public long? ChatRoomId { get; set; }
        public string? Content { get; set; }
        public bool? FromAi { get; set; }
        public bool? Useful { get; set; }
        public string? Sources { get; set; }
    }
}
