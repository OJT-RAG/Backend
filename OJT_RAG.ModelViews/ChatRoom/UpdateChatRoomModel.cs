namespace OJT_RAG.Model.ChatRoomModel
{
    public class UpdateChatRoomModel
    {
        public long ChatRoomId { get; set; }
        public long? UserId { get; set; }
        public string? ChatRoomTitle { get; set; }
        public string? Description { get; set; }
    }
}