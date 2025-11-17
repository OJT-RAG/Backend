namespace OJT_RAG.ModelView.ChatRoomModelView
{
    public class ChatRoomModelView
    {
        public long ChatRoomId { get; set; }
        public long? UserId { get; set; }
        public string? ChatRoomTitle { get; set; }
        public string? Description { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
