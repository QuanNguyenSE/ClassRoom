using ClassRoom.Models;
using ClassRoom.Models.ViewModels;
using ClassRoom.Web.Services;
using ClassRoom.Web.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClassRoom.Web.Controllers
{
	public class CourseController : Controller
	{
		private readonly ICourseService _courseService;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IEnrollmentService _enrollmentService;

		public CourseController(IEnrollmentService enrollmentService, ICourseService courseService, UserManager<ApplicationUser> userManager)
		{
			_courseService = courseService;
			_userManager = userManager;
			_enrollmentService = enrollmentService;
		}

		public async Task<IActionResult> Index(string searchTerm, bool showEnrolled = false)
		{
			var userId = _userManager.GetUserId(User);
			var courses = await _courseService.GetCoursesAsync(searchTerm, showEnrolled, userId);
			return View(courses);
		}
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(CourseCreateViewModel model, IFormFile image)
		{
			if (!ModelState.IsValid)
			{
				TempData["Error"] = "Dữ liệu không hợp lệ.";
				return View(model);
			}

			var result = await _courseService.CreateCourseAsync(model, image);
			if (result)
			{
				TempData["Success"] = "Khóa học đã được tạo thành công!";
				return RedirectToAction(nameof(Index));
			}

			TempData["Error"] = "Có lỗi xảy ra, vui lòng thử lại.";
			return View(model);
		}
		public async Task<IActionResult> Details(int id)
		{
			var userId = _userManager.GetUserId(User); // Lấy ID người dùng hiện tại
			var viewModel = await _courseService.GetCourseDetailsAsync(id, userId);

			if (viewModel == null) return NotFound();

			return View(viewModel);
		}
		public async Task<IActionResult> Update(int id)
		{
			var course = await _courseService.GetCourseForUpdateAsync(id);
			if (course == null) return NotFound();

			return View(course);
		}

		[HttpPost]
		public async Task<IActionResult> Update(CourseUpdateViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			var isUpdated = await _courseService.UpdateCourseAsync(model);

			if (isUpdated)
			{
				TempData["Success"] = "Khóa học đã được cập nhật thành công!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "Có lỗi xảy ra khi cập nhật khóa học.";
			return View(model);
		}
		public async Task<IActionResult> Delete(int id)
		{
			var course = await _courseService.GetCourseForDeleteAsync(id);
			if (course == null) return NotFound();

			return View(course);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var isDeleted = await _courseService.DeleteCourseAsync(id);

			if (isDeleted)
			{
				TempData["Success"] = "Khóa học đã được xóa thành công!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "Không thể xóa khóa học, có thể do lỗi hệ thống hoặc dữ liệu liên quan.";
			return RedirectToAction("Delete", new { id });
		}

		[HttpPost]
		public async Task<IActionResult> Enroll(int courseId)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				TempData["Error"] = "Bạn cần đăng nhập để đăng ký khóa học.";
				return RedirectToAction("Login", "Account");
			}

			bool isEnrolled = await _enrollmentService.EnrollStudentAsync(courseId, user.Id);

			if (isEnrolled)
			{
				TempData["Success"] = "Bạn đã đăng ký khóa học thành công!";
			}
			else
			{
				TempData["Error"] = "Bạn đã đăng ký khóa học này trước đó hoặc có lỗi xảy ra.";
			}

			return RedirectToAction("Details", new { id = courseId });
		}
	}
}
