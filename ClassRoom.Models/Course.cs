using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models;

public enum CourseLevel
{
	Beginner,
	Intermediate,
	Advanced
}

public class Course
{
	[Key]
	public int Id { get; set; }

	[Required, StringLength(200)]
	public string Title { get; set; }

	[Required]
	public int Duration { get; set; }

	[Required]
	public CourseLevel Level { get; set; }
	[ValidateNever]
	public string ImageUrl { get; set; }
	public int MinStudentToOpenClass { get; set; } = 15;

	public DateTime CreatedAt { get; set; } = DateTime.Now;

	public List<Enrollment>? Enrollments { get; set; }
}

