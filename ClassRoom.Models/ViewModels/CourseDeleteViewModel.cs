namespace ClassRoom.Models.ViewModels;

public class CourseDeleteViewModel
{
	public int Id { get; set; }

	public string Title { get; set; }

	public string Description { get; set; }

	public string ImageUrl { get; set; }

	public CourseLevel Level { get; set; }

	public int TotalStudents { get; set; }
}
