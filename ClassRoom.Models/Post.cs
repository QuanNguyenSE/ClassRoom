using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models;

public class Post
{
	[Key]
	public int Id { get; set; }

	[Required]
	public int ClassroomId { get; set; }
	[ForeignKey("ClassroomId")]
	public Classroom Classroom { get; set; }

	[Required]
	public string Content { get; set; } // Nội dung bài post

	public string? FilePath { get; set; } // Nếu có đính kèm file

	[Required]
	public string CreatedById { get; set; }
	[ForeignKey("CreatedById")]
	public ApplicationUser CreatedBy { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.Now;
}
