using ClassRoom.Models.ViewModels;

namespace ClassRoom.Web.Services.IServices;

public interface ICourseService
{
	Task<List<CourseVM>> GetCoursesAsync(string searchTerm, bool showEnrolled, string userId);
	Task<bool> CreateCourseAsync(CourseCreateViewModel model, IFormFile image);
	Task<CourseDetailsViewModel> GetCourseDetailsAsync(int courseId, string userId);
	Task<CourseUpdateViewModel> GetCourseForUpdateAsync(int courseId);
	Task<bool> UpdateCourseAsync(CourseUpdateViewModel model);
	Task<CourseDeleteViewModel?> GetCourseForDeleteAsync(int courseId);
	Task<bool> DeleteCourseAsync(int courseId);
}

