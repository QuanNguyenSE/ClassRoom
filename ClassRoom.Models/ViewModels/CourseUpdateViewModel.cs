using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models.ViewModels;

public class CourseUpdateViewModel
{
	public int Id { get; set; }

	[Required, StringLength(200)]
	public string Title { get; set; }

	public string Description { get; set; }

	[Required]
	public CourseLevel Level { get; set; }

	public IFormFile? Image { get; set; } // Dùng để upload ảnh mới

	public string? ExistingImageUrl { get; set; } // Đường dẫn ảnh hiện tại

	[Required]
	public int MinStudentToOpenClass { get; set; }
}
