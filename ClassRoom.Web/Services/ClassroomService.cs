using ClassRoom.DataAccess.Data;
using ClassRoom.Models;
using ClassRoom.Models.ViewModels;
using ClassRoom.Web.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClassRoom.Web.Services;

public class ClassroomService : IClassroomService
{
	private readonly ApplicationDbContext _context;
	private readonly UserManager<ApplicationUser> _userManager;

	public ClassroomService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
	{
		_context = context;
		_userManager = userManager;
	}

	public async Task<List<ApplicationUser>> GetInstructorsAsync()
	{
		var instructors = await _userManager.GetUsersInRoleAsync("Instructor");
		return instructors.ToList();
	}

	public async Task<List<ApplicationUser>> GetEligibleStudentsAsync(int courseId)
	{
		var enrolledStudentIds = await _context.Enrollments
	   .Where(e => e.CourseId == courseId && e.IsActive)
	   .Select(e => e.StudentId)
	   .ToListAsync();

		var students = await _userManager.GetUsersInRoleAsync("Student");

		return students.Where(s => enrolledStudentIds.Contains(s.Id)).ToList();
	}

	public async Task<bool> CreateClassroomAsync(Classroom classroom, List<string> studentIds)
	{
		// Kiểm tra course hợp lệ
		var course = await _context.Courses.FindAsync(classroom.CourseId);
		if (course == null || studentIds.Count < course.MinStudentToOpenClass)
			return false;

		// Thêm lớp học
		_context.Classrooms.Add(classroom);
		await _context.SaveChangesAsync();

		// Thêm sinh viên vào lớp học
		var students = await _context.Users
			.Where(s => studentIds.Contains(s.Id))
			.ToListAsync();

		var classroomStudents = students.Select(s => new ClassroomStudent
		{
			ClassroomId = classroom.Id,
			StudentId = s.Id,
			IsActive = true // Cập nhật trạng thái khi đã vào lớp
		}).ToList();

		_context.ClassroomStudents.AddRange(classroomStudents);
		await _context.SaveChangesAsync();

		return true;
	}
	// Instructor - Hiển thị lớp mà instructor đang dạy
	public async Task<List<Classroom>> GetClassroomsForInstructorAsync(string instructorId)
	{
		return await _context.Classrooms
			.Where(c => c.InstructorId == instructorId)
			.Include(c => c.Course)
			.ToListAsync();
	}

	// Student - Hiển thị lớp mà student đang tham gia
	public async Task<List<Classroom>> GetClassroomsForStudentAsync(string studentId)
	{
		return await _context.Classrooms
			.Where(c => c.ClassroomStudents.Any(cs => cs.StudentId == studentId))
			.Include(c => c.Instructor)
			.Include(c => c.Course)
			.ToListAsync();
	}

	// Staff - Hiển thị tất cả các lớp học
	public async Task<List<Classroom>> GetAllClassroomsForStaffAsync()
	{
		return await _context.Classrooms
			.Include(c => c.Course)
			.Include(c => c.Instructor)
			.ToListAsync();
	}

	public async Task<ClassroomDetailViewModel?> GetClassroomDetailAsync(int classroomId)
	{
		var classroom = await _context.Classrooms
			.Include(c => c.Instructor)
			.Include(c => c.ClassroomStudents)
				.ThenInclude(cs => cs.Student)
			.Include(c => c.Assignments)
			.FirstOrDefaultAsync(c => c.Id == classroomId);

		if (classroom == null)
			return null;

		return new ClassroomDetailViewModel
		{
			Id = classroom.Id,
			Name = classroom.Name,
			Information = classroom.Information,
			InstructorName = classroom.Instructor.UserName,
			Student = classroom.ClassroomStudents
			.Where(cs => cs.IsActive)
			.Select(cs => new StudentViewModel
			{
				Id = cs.StudentId,
				Name = cs.Student.FullName,
				Email = cs.Student.Email
			}).ToList(),
			Assignments = classroom.Assignments.Select(a => new AssignmentViewModel
			{
				Id = a.Id,
				Title = a.Title,
				Description = a.Description,
				DueDate = a.DueDate
			}).ToList()
		};
	}
	public async Task<int?> GetClassroomIdByAssignmentIdAsync(int assignmentId)
	{
		return await _context.Assignments
			.Where(a => a.Id == assignmentId)
			.Select(a => (int?)a.ClassroomId)
			.FirstOrDefaultAsync();
	}

	//public async Task<bool> CreatePostAsync(Post post)
	//{
	//	_context.Posts.Add(post);
	//	return await _context.SaveChangesAsync() > 0;
	//}

	//public async Task<bool> CreateAssignmentAsync(Assignment assignment)
	//{
	//	_context.Assignments.Add(assignment);
	//	return await _context.SaveChangesAsync() > 0;
	//}

	//public async Task<bool> SubmitAssignmentAsync(Submission submission)
	//{
	//	_context.Submissions.Add(submission);
	//	return await _context.SaveChangesAsync() > 0;
	//}

	//public async Task<bool> GradeSubmissionAsync(int submissionId, float grade)
	//{
	//	var submission = await _context.Submissions.FindAsync(submissionId);
	//	if (submission == null) return false;

	//	submission.Grade = grade;
	//	return await _context.SaveChangesAsync() > 0;
	//}

}
