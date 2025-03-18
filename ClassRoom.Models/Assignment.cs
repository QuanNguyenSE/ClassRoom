using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClassRoom.Models;

public class Assignment
{
	[Key]
	public int Id { get; set; }

	[Required]
	public int ClassroomId { get; set; }
	[ForeignKey("ClassroomId")]
	[ValidateNever]
	public Classroom Classroom { get; set; }

	[Required]
	public string Title { get; set; } // Tiêu đề bài tập

	public string Description { get; set; } // Mô tả bài tập

	public string? FilePath { get; set; } // Nếu có file đính kèm (PDF, Word)

	public DateTime DueDate { get; set; } // Hạn nộp

	[Required]
	public string CreatedById { get; set; }
	[ForeignKey("CreatedById")]
	[ValidateNever]

	public ApplicationUser CreatedBy { get; set; }

	public List<Submission>? Submissions { get; set; } = new();
}
