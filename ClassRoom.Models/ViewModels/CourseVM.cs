using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClassRoom.Models.ViewModels;

public class CourseVM
{
	public int Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public decimal Price { get; set; }
	public int Duration { get; set; }
	public CourseLevel Level { get; set; }
	
	public string ImageUrl { get; set; }
	public bool IsRegistered { get; set; }  // ✅ Check nếu user đã đăng ký
}

