using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models.ViewModels;

public class CourseCreateViewModel
{
	[Required, StringLength(200)]
	public string Title { get; set; }
	public string Description { get; set; }
	[Required]
	public CourseLevel Level { get; set; }
	[ValidateNever]
	public string ImageUrl { get; set; }
	[Required]
	public int MinStudentToOpenClass { get; set; }
}