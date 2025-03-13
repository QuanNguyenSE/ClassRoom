using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models;

public class Classroom
{
	[Key]
	public int Id { get; set; }
	[Required, StringLength(100)]
	public string Name { get; set; }

	[Required]
	public int CourseId { get; set; }
	[ForeignKey("CourseId")]
	public Course Course { get; set; }

	[Required]
	public string InstructorId { get; set; }
	[ForeignKey("InstructorId")]
	public ApplicationUser Instructor { get; set; }

	public DateTime CreatedDate { get; set; } = DateTime.Now;

	public List<ApplicationUser> Students { get; set; } = new();
}

