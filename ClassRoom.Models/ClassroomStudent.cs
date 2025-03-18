using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models;

public class ClassroomStudent
{
	[Key]
	public int Id { get; set; }

	[Required]
	public int ClassroomId { get; set; }
	[ForeignKey("ClassroomId")]
	public Classroom Classroom { get; set; }

	[Required]
	public string StudentId { get; set; }
	[ForeignKey("StudentId")]
	public ApplicationUser Student { get; set; }

	// Trạng thái active để xác định student có đang tham gia lớp này hay không
	public bool IsActive { get; set; } = true;
}
