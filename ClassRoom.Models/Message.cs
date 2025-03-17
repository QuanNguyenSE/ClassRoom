using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models;

public class Message
{
	public int Id { get; set; }

	[Required]
	public string UserId { get; set; } // ID người gửi

	[ForeignKey("UserId")]
	public ApplicationUser User { get; set; } // Người gửi tin nhắn

	[Required]
	public int ChatRoomId { get; set; } // Room chat mà tin nhắn này thuộc về

	[ForeignKey("ChatRoomId")]
	public ChatRoom ChatRoom { get; set; }

	[Required]
	public string Content { get; set; } // Nội dung tin nhắn

	public DateTime SentAt { get; set; } = DateTime.Now; // Thời gian gửi tin nhắn
}