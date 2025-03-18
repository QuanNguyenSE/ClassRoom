using ClassRoom.Models.ViewModels;
using ClassRoom.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClassRoom.Web.Controllers
{
	public class SubmissionController : Controller
	{
		private readonly ISubmissionService _submissionService;
		private readonly IClassroomService _classroomService;

		public SubmissionController(ISubmissionService submissionService, IClassroomService classroomService)
		{
			_submissionService = submissionService;
			_classroomService = classroomService;
		}
		[Authorize(Roles = "Instructor")]
		public async Task<IActionResult> Index(int assignmentId)
		{
			var submissions = await _submissionService.GetSubmissionsByAssignmentAsync(assignmentId);
			if (submissions == null) return NotFound();

			ViewBag.AssignmentId = assignmentId; // Để dùng cho breadcrumb hoặc hiển thị tên bài tập
			return View(submissions);
		}
		[Authorize(Roles = "Student")]

		public async Task<IActionResult> Submit(int assignmentId)
		{
			//var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			//var model = await _submissionService.GetSubmissionFormAsync(assignmentId, studentId);
			//if (model == null) return NotFound();

			//return View(model);

			var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var assignmentInfo = await _submissionService.GetSubmissionFormAsync(assignmentId, studentId);

			if (assignmentInfo == null) return NotFound();

			var model = new SubmissionCreateModel
			{
				AssignmentId = assignmentId,
				StudentId = studentId,
			};

			ViewBag.AssignmentTitle = assignmentInfo.AssignmentTitle;
			ViewBag.ClassroomName = assignmentInfo.ClassroomName;
			ViewBag.DueDate = assignmentInfo.DueDate;

			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "Student")]

		public async Task<IActionResult> Submit(SubmissionCreateModel model)
		{
			if (!ModelState.IsValid) return View(model);

			var result = await _submissionService.SubmitAssignmentAsync(model);
			if (!result) return BadRequest("Submission failed");
			var classid = await _classroomService.GetClassroomIdByAssignmentIdAsync(model.AssignmentId);

			return RedirectToAction("Details", "Classroom", new { id = classid });
		}
		[Authorize(Roles = "Instructor")]

		public async Task<IActionResult> GradeSubmission(int submissionId)
		{
			var submissionDetail = await _submissionService.GetSubmissionDetailAsync(submissionId);
			if (submissionDetail == null) return NotFound();

			//var model = new SubmissionUpdateModel
			//{
			//	SubmissionId = submissionDetail.SubmissionId,
			//	Grade = submissionDetail.Grade ?? 0
			//};

			return View(submissionDetail);
		}

		[HttpPost]
		[Authorize(Roles = "Instructor")]

		public async Task<IActionResult> GradeSubmission(SubmissionUpdateModel model)
		{
			if (!ModelState.IsValid) return View(model);

			var result = await _submissionService.UpdateSubmissionAsync(model);
			if (!result) return BadRequest("Update failed");

			return RedirectToAction("Index", "Submission", new { assignmentId = model.AssignmentId });
		}
	}
}
