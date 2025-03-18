using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models.ViewModels;

public class SubmissionDetailViewModel
{
	public int SubmissionId { get; set; }
	public int AssignmentId { get; set; }
	public string AssignmentTitle { get; set; }
	public string StudentName { get; set; }
	public string? Answer { get; set; }
	public string? FilePath { get; set; }
	public DateTime SubmittedAt { get; set; }
	public float? Grade { get; set; }
}

public class SubmissionUpdateModel
{
	public int SubmissionId { get; set; }
	[Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
	public int AssignmentId { get; set; }

	public float Grade { get; set; }
}
