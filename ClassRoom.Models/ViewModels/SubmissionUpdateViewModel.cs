namespace ClassRoom.Models.ViewModels;

public class SubmissionUpdateViewModel
{
	public int SubmissionId { get; set; }
	public string Answer { get; set; }
	public string StudentName { get; set; }
	public DateTime SubmittedAt { get; set; }
	public float? Grade { get; set; }
}
