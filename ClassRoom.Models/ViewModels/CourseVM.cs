namespace ClassRoom.Models.ViewModels;

public class CourseVM
{
	public int Id { get; set; }
	public string Title { get; set; }
	public CourseLevel Level { get; set; }
	public string ImageUrl { get; set; }
	public DateTime CreatedAt { get; set; }
	public List<Enrollment>? Enrollments { get; set; }
	public int MinStudentToOpenClass { get; set; }
	public bool IsEnrolled { get; set; } // Check xem user đã đăng ký khóa học hay chưa
}
