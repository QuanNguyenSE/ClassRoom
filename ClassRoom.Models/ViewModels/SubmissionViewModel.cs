using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClassRoom.Models.ViewModels;

public class SubmissionViewModel
{
	public int AssignmentId { get; set; }
	public string AssignmentTitle { get; set; }
	public string ClassroomName { get; set; }
	public DateTime DueDate { get; set; }
}

public class SubmissionCreateModel
{
	public int AssignmentId { get; set; }
	public string StudentId { get; set; }
	public string? Answer { get; set; }
	public IFormFile? File { get; set; }
	[ValidateNever]

	public AssignmentViewModel Assignment { get; set; }
}
