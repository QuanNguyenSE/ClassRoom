namespace ClassRoom.Models.ViewModels;

public class CourseDetailsViewModel
{
	public Course Course { get; set; }
	public int ActiveEnrollments { get; set; }  // Số sinh viên đã đăng ký nhưng chưa học
	public int CompletedEnrollments { get; set; }  // Số sinh viên đã hoàn thành khóa học
	public string UserRole { get; set; }  // Vai trò của người dùng hiện tại
}
