namespace OJT_RAG.DTOs.ChatRoomDTO
{
    public class UpdateChatRoomDTO
    {
        public long ChatRoomId { get; set; }
        public long? UserId { get; set; }
        public string? ChatRoomTitle { get; set; }
        public string? Description { get; set; }
    }
}
