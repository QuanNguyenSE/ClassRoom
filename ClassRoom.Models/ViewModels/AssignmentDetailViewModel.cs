namespace ClassRoom.Models.ViewModels;

public class AssignmentDetailViewModel
{
	public int Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string? FilePath { get; set; }
	public DateTime DueDate { get; set; }
	public string CreatedBy { get; set; }
}
