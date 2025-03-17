using ClassRoom.DataAccess.Data;
using ClassRoom.Models;
using ClassRoom.Web.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace ClassRoom.Web.Services;

public class EnrollmentService : IEnrollmentService
{
	private readonly ApplicationDbContext _context;

	public EnrollmentService(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<bool> EnrollStudentAsync(int courseId, string studentId)
	{
		var course = await _context.Courses.FindAsync(courseId);
		if (course == null) return false;

		// Kiểm tra xem sinh viên đã đăng ký khóa học chưa
		bool alreadyEnrolled = await _context.Enrollments
			.AnyAsync(e => e.CourseId == courseId && e.StudentId == studentId);

		if (alreadyEnrolled) return false;

		var enrollment = new Enrollment
		{
			CourseId = courseId,
			StudentId = studentId,
			IsActive = true,
			EnrollmentDate = DateTime.Now
		};

		_context.Enrollments.Add(enrollment);
		await _context.SaveChangesAsync();
		return true;
	}
}
