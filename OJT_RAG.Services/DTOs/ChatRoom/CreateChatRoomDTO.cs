namespace OJT_RAG.DTOs.ChatRoomDTO
{
    public class CreateChatRoomDTO
    {
        public long? UserId { get; set; }
        public string? ChatRoomTitle { get; set; }
        public string? Description { get; set; }
    }
}
