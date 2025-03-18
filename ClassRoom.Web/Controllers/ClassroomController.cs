using ClassRoom.Models;
using ClassRoom.Models.ViewModels;
using ClassRoom.Utility;
using ClassRoom.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClassRoom.Web.Controllers
{
	[Authorize]
	public class ClassroomController : Controller
	{
		private readonly IAssignmentService _assignmentService;
		private readonly IClassroomService _classroomService;

		public ClassroomController(IAssignmentService assignmentService, IClassroomService classroomService)
		{
			_assignmentService = assignmentService;
			_classroomService = classroomService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

			List<Classroom> classrooms = new();

			if (userRoles.Contains("Staff"))
			{
				classrooms = await _classroomService.GetAllClassroomsForStaffAsync();
				return View("Index_Staff", classrooms);
			}
			else if (userRoles.Contains("Instructor"))
			{
				classrooms = await _classroomService.GetClassroomsForInstructorAsync(userId);
				return View("Index_Instructor", classrooms);
			}
			else if (userRoles.Contains("Student"))
			{
				classrooms = await _classroomService.GetClassroomsForStudentAsync(userId);
				return View("Index_Student", classrooms);
			}

			return Unauthorized();
		}


		[HttpGet]
		[Authorize(Roles = "Staff")]
		public async Task<IActionResult> Create(int courseId)
		{
			var instructors = await _classroomService.GetInstructorsAsync();
			var students = await _classroomService.GetEligibleStudentsAsync(courseId);

			var viewModel = new ClassroomCreateViewModel
			{
				CourseId = courseId,
				Instructors = instructors,
				Students = students
			};

			return View(viewModel);
		}

		[HttpPost]
		[Authorize(Roles = "Staff")]
		public async Task<IActionResult> Create(ClassroomCreateViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			var classroom = new Classroom
			{
				Name = model.Name,
				Information = model.Information,
				CourseId = model.CourseId,
				InstructorId = model.InstructorId
			};

			var result = await _classroomService.CreateClassroomAsync(classroom, model.SelectedStudentIds);

			if (!result)
			{
				ModelState.AddModelError("", "Không đủ số lượng sinh viên để mở lớp.");
				return View(model);
			}

			return RedirectToAction("Index");
		}

		//public async Task<IActionResult> Details(int id)
		//{
		//	var classroom = await _classroomService.GetClassroomDetailAsync(id);
		//	if (classroom == null)
		//	{
		//		return NotFound();
		//	}
		//	return View(classroom);
		//}
		public async Task<IActionResult> Details(int id)
		{
			var classroom = await _classroomService.GetClassroomDetailAsync(id);
			if (classroom == null) return NotFound();

			var assignments = await _assignmentService.GetAssignmentsAsync(id);
			var viewModel = new ClassroomDetailsViewModel
			{
				Classroom = classroom,
				Assignments = assignments
			};
			return View(viewModel);
		}

		[HttpPost]
		[Authorize(Roles = "Instructor")]
		public async Task<IActionResult> CreateAssignment(int classroomId, Assignment assignment, IFormFile? file)
		{
			assignment.ClassroomId = classroomId;

			if (!ModelState.IsValid) return RedirectToAction("Details", new { id = classroomId });

			await _assignmentService.CreateAssignmentAsync(assignment, file);
			return RedirectToAction("Details", new { id = classroomId });
		}

		[HttpPost]
		[Authorize(Roles = "Instructor")]

		public async Task<IActionResult> UpdateAssignment(int classroomId, Assignment assignment, IFormFile? file)
		{

			await _assignmentService.UpdateAssignmentAsync(assignment, file);
			return RedirectToAction("Details", new { id = classroomId });
		}

		[HttpPost]
		[Authorize(Roles = "Instructor")]

		public async Task<IActionResult> DeleteAssignment(int classroomId, int assignmentId)
		{
			await _assignmentService.DeleteAssignmentAsync(assignmentId);
			return RedirectToAction("Details", new { id = classroomId });
		}
		public async Task<IActionResult> GetAssignment(int id)
		{
			var assignment = await _assignmentService.GetAssignmentAsync(id);
			if (assignment == null) return NotFound();

			return PartialView("_ViewAssignmentModal", assignment);
		}

	}
}
