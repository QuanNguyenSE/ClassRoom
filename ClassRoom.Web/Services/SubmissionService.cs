using ClassRoom.DataAccess.Data;
using ClassRoom.Models;
using ClassRoom.Models.ViewModels;
using ClassRoom.Web.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace ClassRoom.Web.Services;

public class SubmissionService : ISubmissionService
{
	private readonly ApplicationDbContext _context;
	private readonly IWebHostEnvironment _webHostEnvironment;

	public SubmissionService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
	{
		_context = context;
		_webHostEnvironment = webHostEnvironment;
	}

	public async Task<SubmissionViewModel?> GetSubmissionFormAsync(int assignmentId, string studentId)
	{
		var assignment = await _context.Assignments
			.Include(a => a.Classroom)
			.FirstOrDefaultAsync(a => a.Id == assignmentId);

		if (assignment == null) return null;

		return new SubmissionViewModel
		{
			AssignmentId = assignment.Id,
			AssignmentTitle = assignment.Title,
			ClassroomName = assignment.Classroom.Name,
			DueDate = assignment.DueDate
		};
	}

	public async Task<bool> SubmitAssignmentAsync(SubmissionCreateModel model)
	{
		var submission = new Submission
		{
			AssignmentId = model.AssignmentId,
			StudentId = model.StudentId,
			Answer = model.Answer,
			SubmittedAt = DateTime.Now
		};

		if (model.File != null)
		{
			var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "submissions");
			Directory.CreateDirectory(uploadsFolder);

			var fileName = $"{Guid.NewGuid()}_{model.File.FileName}";
			var filePath = Path.Combine(uploadsFolder, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await model.File.CopyToAsync(stream);
			}

			submission.FilePath = $"/submissions/{fileName}";
		}

		_context.Submissions.Add(submission);
		await _context.SaveChangesAsync();
		return true;
	}
	public bool HasSubmittedAssignment(string studentId, int assignmentId)
	{
		return _context.Submissions
			.Any(s => s.StudentId == studentId && s.AssignmentId == assignmentId);
	}
	public async Task<SubmissionDetailViewModel?> GetSubmissionDetailAsync(int submissionId)
	{
		var submission = await _context.Submissions
			.Include(s => s.Assignment)
			.Include(s => s.Student)
			.FirstOrDefaultAsync(s => s.Id == submissionId);

		if (submission == null) return null;

		return new SubmissionDetailViewModel
		{
			SubmissionId = submission.Id,
			AssignmentId = submission.AssignmentId,
			AssignmentTitle = submission.Assignment.Title,
			StudentName = submission.Student.FullName,
			Answer = submission.Answer,
			FilePath = submission.FilePath,
			SubmittedAt = submission.SubmittedAt,
			Grade = submission.Grade
		};
	}

	public async Task<bool> UpdateSubmissionAsync(SubmissionUpdateModel model)
	{
		var submission = await _context.Submissions.FindAsync(model.SubmissionId);
		if (submission == null) return false;

		submission.Grade = model.Grade;
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<List<SubmissionUpdateViewModel>> GetSubmissionsByAssignmentAsync(int assignmentId)
	{
		var submissions = await _context.Submissions
			.Where(s => s.AssignmentId == assignmentId)
			.Include(s => s.Student)
			.Select(s => new SubmissionUpdateViewModel
			{
				SubmissionId = s.Id,
				StudentName = s.Student.FullName,
				Answer = s.Answer,
				SubmittedAt = s.SubmittedAt,
				Grade = s.Grade
			})
			.ToListAsync();

		return submissions;
	}
}
