using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Repositories.Entities;

[Table("message")]
public partial class Message
{
    [Key]
    [Column("message_id")]
    public long MessageId { get; set; }

    [Column("chat_room_id")]
    public long? ChatRoomId { get; set; }

    [Column("content")]
    public string? Content { get; set; }

    [Column("from_ai")]
    public bool? FromAi { get; set; }

    [Column("useful")]
    public bool? Useful { get; set; }

    [Column("sources")]
    public string? Sources { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ChatRoomId")]
    [InverseProperty("Messages")]
    public virtual ChatRoom? ChatRoom { get; set; }
}
