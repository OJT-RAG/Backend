    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace OJT_RAG.Repositories.Entities
    {
        [Table("user_chat_message")]
        public class UserChatMessage
        {
            [Key]
            [Column("user_chat_message_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

            [Column("sender_id")]
            public long SenderId { get; set; }

            [Column("receiver_id")]
            public long ReceiverId { get; set; }

            [Column("content")]
            public string Content { get; set; } = null!;

            [Column("sent_at")]
            public DateTime SentAt { get; set; }
        }
    }
