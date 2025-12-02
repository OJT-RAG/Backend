namespace OJT_RAG.Model.ChatRoomModel
{
    public class CreateChatRoomModel
    {
        public long? UserId { get; set; }
        public string? ChatRoomTitle { get; set; }
        public string? Description { get; set; }
    }
}