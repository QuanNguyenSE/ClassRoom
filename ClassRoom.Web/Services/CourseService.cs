using ClassRoom.DataAccess.Data;
using ClassRoom.Models;
using ClassRoom.Models.ViewModels;
using ClassRoom.Web.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClassRoom.Web.Services;

public class CourseService : ICourseService
{
	private readonly ApplicationDbContext _db;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IWebHostEnvironment _webHostEnvironment;

	public CourseService(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
	{
		_db = db;
		_userManager = userManager;
		_webHostEnvironment = webHostEnvironment;
	}

	public async Task<List<CourseVM>> GetCoursesAsync(string searchTerm, bool showEnrolled, string userId)
	{
		var isAuthenticated = !string.IsNullOrEmpty(userId);

		var courses = _db.Courses
			.Include(c => c.Enrollments)
			.Select(c => new CourseVM
			{
				Id = c.Id,
				Title = c.Title,
				Level = c.Level,
				ImageUrl = c.ImageUrl,
				CreatedAt = c.CreatedAt,
				Enrollments = c.Enrollments,
				MinStudentToOpenClass = c.MinStudentToOpenClass,
				IsEnrolled = isAuthenticated && c.Enrollments.Any(e => e.StudentId == userId)
			});

		if (!string.IsNullOrEmpty(searchTerm))
		{
			courses = courses.Where(c => c.Title.Contains(searchTerm));
		}

		if (showEnrolled && isAuthenticated)
		{
			courses = courses.Where(c => c.IsEnrolled);
		}

		return await courses.ToListAsync();
	}
	public async Task<bool> CreateCourseAsync(CourseCreateViewModel model, IFormFile image)
	{
		if (image != null)
		{
			string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
			string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
			string filePath = Path.Combine(uploadsFolder, uniqueFileName);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await image.CopyToAsync(fileStream);
			}

			model.ImageUrl = "/images/" + uniqueFileName;
		}

		var course = new Course
		{
			Title = model.Title,
			Description = model.Description,
			Level = model.Level,
			ImageUrl = model.ImageUrl,
			MinStudentToOpenClass = model.MinStudentToOpenClass,
			CreatedAt = DateTime.Now
		};

		_db.Courses.Add(course);
		await _db.SaveChangesAsync();
		return true;
	}
	public async Task<CourseDetailsViewModel> GetCourseDetailsAsync(int courseId, string userId)
	{
		var course = await _db.Courses
		.Include(c => c.Enrollments)
		.FirstOrDefaultAsync(c => c.Id == courseId);

		if (course == null) return null;

		string userRole = "Anonymous"; // Mặc định là Anonymous

		if (!string.IsNullOrEmpty(userId))
		{
			var user = await _userManager.FindByIdAsync(userId);
			var roles = await _userManager.GetRolesAsync(user);
			userRole = roles.FirstOrDefault() ?? "Anonymous";
		}

		int activeEnrollments = course.Enrollments.Count(e => e.IsActive);
		int completedEnrollments = course.Enrollments.Count(e => !e.IsActive);

		return new CourseDetailsViewModel
		{
			Course = course,
			ActiveEnrollments = activeEnrollments,
			CompletedEnrollments = completedEnrollments,
			UserRole = userRole
		};
	}
	public async Task<CourseUpdateViewModel> GetCourseForUpdateAsync(int courseId)
	{
		var course = await _db.Courses.FindAsync(courseId);
		if (course == null) return null;

		return new CourseUpdateViewModel
		{
			Id = course.Id,
			Title = course.Title,
			Description = course.Description,
			Level = course.Level,
			ExistingImageUrl = course.ImageUrl,
			MinStudentToOpenClass = course.MinStudentToOpenClass
		};
	}

	public async Task<bool> UpdateCourseAsync(CourseUpdateViewModel model)
	{
		var course = await _db.Courses.FindAsync(model.Id);
		if (course == null) return false;

		course.Title = model.Title;
		course.Description = model.Description;
		course.Level = model.Level;
		course.MinStudentToOpenClass = model.MinStudentToOpenClass;

		// Xử lý upload ảnh mới
		if (model.Image != null)
		{
			var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
			var uniqueFileName = $"{Guid.NewGuid()}_{model.Image.FileName}";
			var filePath = Path.Combine(uploadsFolder, uniqueFileName);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await model.Image.CopyToAsync(fileStream);
			}

			// Xóa ảnh cũ nếu có
			if (!string.IsNullOrEmpty(course.ImageUrl))
			{
				var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, course.ImageUrl.TrimStart('/'));
				if (File.Exists(oldImagePath))
				{
					File.Delete(oldImagePath);
				}
			}

			course.ImageUrl = "/images/" + uniqueFileName;
		}

		_db.Courses.Update(course);
		await _db.SaveChangesAsync();
		return true;
	}
	public async Task<CourseDeleteViewModel?> GetCourseForDeleteAsync(int courseId)
	{
		var course = await _db.Courses
			.Include(c => c.Enrollments)
			.FirstOrDefaultAsync(c => c.Id == courseId);

		if (course == null) return null;

		return new CourseDeleteViewModel
		{
			Id = course.Id,
			Title = course.Title,
			Description = course.Description,
			ImageUrl = course.ImageUrl,
			Level = course.Level,
			TotalStudents = course.Enrollments.Count
		};
	}

	public async Task<bool> DeleteCourseAsync(int courseId)
	{
		var course = await _db.Courses.FindAsync(courseId);
		if (course == null) return false;

		// Xóa ảnh nếu có
		if (!string.IsNullOrEmpty(course.ImageUrl))
		{
			var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, course.ImageUrl.TrimStart('/'));
			if (File.Exists(imagePath))
			{
				File.Delete(imagePath);
			}
		}

		_db.Courses.Remove(course);
		await _db.SaveChangesAsync();
		return true;
	}
}
