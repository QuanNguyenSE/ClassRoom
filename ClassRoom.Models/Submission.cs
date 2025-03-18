using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models;

public class Submission
{
	[Key]
	public int Id { get; set; }

	[Required]
	public int AssignmentId { get; set; }
	[ForeignKey("AssignmentId")]
	public Assignment Assignment { get; set; }

	[Required]
	public string StudentId { get; set; }
	[ForeignKey("StudentId")]
	public ApplicationUser Student { get; set; }
	public string? Answer { get; set; } // Bình luận của Student

	public string? FilePath { get; set; } // Đường dẫn file bài nộp

	public float? Grade { get; set; } // Điểm số (Instructor chấm)

	public DateTime SubmittedAt { get; set; } = DateTime.Now;
}
