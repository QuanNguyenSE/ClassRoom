namespace ClassRoom.Models.ViewModels;

public class ClassroomDetailViewModel
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Information { get; set; }
	public string InstructorName { get; set; }
	public List<StudentViewModel> Student { get; set; } = new();
	public List<AssignmentViewModel> Assignments { get; set; } = new();
}

public class AssignmentViewModel
{
	public int Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public DateTime DueDate { get; set; }
}