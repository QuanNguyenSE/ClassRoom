using ClassRoom.Models;
using ClassRoom.Web.Models;
using ClassRoom.Web.Services;
using ClassRoom.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClassRoom.Web.Controllers
{
	[AllowAnonymous]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ICourseService _courseService;
		private readonly UserManager<ApplicationUser> _userManager;

		public HomeController(ILogger<HomeController> logger,ICourseService courseService, UserManager<ApplicationUser> userManager)
		{
			_logger = logger;
			_courseService = courseService;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index(string searchTerm, bool showEnrolled = false)
		{
			var userId = _userManager.GetUserId(User);
			var courses = await _courseService.GetCoursesAsync(searchTerm, showEnrolled, userId);

			return View(courses);
		}

		[Authorize(Roles = "Admin")]
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
