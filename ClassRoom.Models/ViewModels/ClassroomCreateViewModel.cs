using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models.ViewModels;

public class ClassroomCreateViewModel
{
	[Required, StringLength(100)]
	public string Name { get; set; }

	public string Information { get; set; }

	[Required]
	public int CourseId { get; set; }

	[Required]
	public string InstructorId { get; set; }

	public List<ApplicationUser> Instructors { get; set; } = new();
	public List<ApplicationUser> Students { get; set; } = new();

	public List<string> SelectedStudentIds { get; set; } = new();
}
