using ClassRoom.Models;
using ClassRoom.Models.ViewModels;

namespace ClassRoom.Web.Services.IServices;

public interface IAssignmentService
{
	Task<List<Assignment>> GetAssignmentsAsync(int classroomId);
	Task<AssignmentDetailViewModel?> GetAssignmentAsync(int id);
	Task<Assignment?> GetAssignmentByIdAsync(int id);
	Task<bool> CreateAssignmentAsync(Assignment assignment, IFormFile? file);
	Task<bool> UpdateAssignmentAsync(Assignment assignment, IFormFile? file);
	Task<bool> DeleteAssignmentAsync(int id);
}

