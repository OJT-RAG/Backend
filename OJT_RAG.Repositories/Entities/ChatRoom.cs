using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Repositories.Entities;

[Table("chat_room")]
public partial class ChatRoom
{
    [Key]
    [Column("chat_room_id")]
    public long ChatRoomId { get; set; }

    [Column("user_id")]
    public long? UserId { get; set; }

    [Column("chat_room_title")]
    [StringLength(255)]
    public string? ChatRoomTitle { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("create_at", TypeName = "timestamp without time zone")]
    public DateTime? CreateAt { get; set; }

    [Column("update_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdateAt { get; set; }

    [InverseProperty("ChatRoom")]
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    [ForeignKey("UserId")]
    [InverseProperty("ChatRooms")]
    public virtual User? User { get; set; }
}
