namespace OJT_RAG.Model.MessageModel
{
    public class CreateMessageModel
    {
        public long? ChatRoomId { get; set; }
        public string? Content { get; set; }
        public bool? FromAi { get; set; }
        public bool? Useful { get; set; }
        public string? Sources { get; set; }
    }
}
