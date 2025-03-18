using ClassRoom.Models;
using ClassRoom.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassRoom.Web.Controllers
{
	[Authorize]
	public class AssignmentController : Controller
	{
		private readonly IAssignmentService _assignmentService;

		public AssignmentController(IAssignmentService assignmentService)
		{
			_assignmentService = assignmentService;
		}

		public async Task<IActionResult> Index(int classroomId)
		{
			var assignments = await _assignmentService.GetAssignmentsAsync(classroomId);
			return View(assignments);
		}
		[Authorize(Roles = "Instructor")]
		public IActionResult Create(int classroomId)
		{
			return View(new Assignment { ClassroomId = classroomId });
		}

		[HttpPost]
		[Authorize(Roles = "Instructor")]

		public async Task<IActionResult> Create(Assignment assignment, IFormFile? file)
		{
			if (!ModelState.IsValid) return View(assignment);

			await _assignmentService.CreateAssignmentAsync(assignment, file);
			return RedirectToAction("Index", new { classroomId = assignment.ClassroomId });
		}
		[Authorize(Roles = "Instructor")]

		public async Task<IActionResult> Edit(int id)
		{
			var assignment = await _assignmentService.GetAssignmentByIdAsync(id);
			if (assignment == null) return NotFound();

			return View(assignment);
		}

		[HttpPost]
		[Authorize(Roles = "Instructor")]

		public async Task<IActionResult> Edit(Assignment assignment, IFormFile? file)
		{
			if (!ModelState.IsValid) return View(assignment);

			await _assignmentService.UpdateAssignmentAsync(assignment, file);
			return RedirectToAction("Index", new { classroomId = assignment.ClassroomId });
		}
		[Authorize(Roles = "Instructor")]

		public async Task<IActionResult> Delete(int id)
		{
			var success = await _assignmentService.DeleteAssignmentAsync(id);
			return RedirectToAction("Index");
		}
	}
}
