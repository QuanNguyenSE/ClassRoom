using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models.ViewModels;

public class ClassroomCreateVM
{
	[ValidateNever]
	public Course Course { get; set; }
	[Required]
	public int CourseId { get; set; }

	[Required, StringLength(100)]
	public string Name { get; set; }

	[Required]
	public string InstructorId { get; set; }
	[ValidateNever]
	public List<StudentSelectionVM> Students { get; set; } = new();
	public List<SelectListItem> Instructors { get; set; } = new();
}

public class StudentSelectionVM
{
	public string StudentId { get; set; }
	public string FullName { get; set; }
	public string Email { get; set; }
	public bool Selected { get; set; }
}
