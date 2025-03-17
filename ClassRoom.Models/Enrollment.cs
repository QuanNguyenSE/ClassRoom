using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassRoom.Models;

public class Enrollment
{
	[Key]
	public int Id { get; set; }

	[Required]
	public string StudentId { get; set; }
	[ForeignKey("StudentId")]
	public ApplicationUser Student { get; set; }

	public int CourseId { get; set; }
	[ForeignKey("CourseId")]
	public Course Course { get; set; }
	public bool IsActive { get; set; } = true;

	public DateTime EnrollmentDate { get; set; } = DateTime.Now;
}
