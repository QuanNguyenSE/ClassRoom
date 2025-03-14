namespace ClassRoom.Models.ViewModels;

public class CourseVM
{
	public int Id { get; set; }
	public string Title { get; set; }
	public int Duration { get; set; }
	public CourseLevel Level { get; set; }
	public string ImageUrl { get; set; }

	public DateTime CreatedAt { get; set; }
	public int StudentCount { get; set; }
}

