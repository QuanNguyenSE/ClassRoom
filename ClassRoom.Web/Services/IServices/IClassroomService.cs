using ClassRoom.Models;
using ClassRoom.Models.ViewModels;

namespace ClassRoom.Web.Services.IServices;

public interface IClassroomService
{
	Task<List<ApplicationUser>> GetInstructorsAsync();
	Task<List<ApplicationUser>> GetEligibleStudentsAsync(int courseId);
	Task<bool> CreateClassroomAsync(Classroom classroom, List<string> studentIds);
	Task<List<Classroom>> GetClassroomsForInstructorAsync(string instructorId);
	Task<List<Classroom>> GetClassroomsForStudentAsync(string studentId);
	Task<List<Classroom>> GetAllClassroomsForStaffAsync();

	Task<ClassroomDetailViewModel?> GetClassroomDetailAsync(int classroomId);
	Task<int?> GetClassroomIdByAssignmentIdAsync(int assignmentId);
	//Task<bool> CreatePostAsync(Post post);
	//Task<bool> CreateAssignmentAsync(Assignment assignment);
	//Task<bool> SubmitAssignmentAsync(Submission submission);
	//Task<bool> GradeSubmissionAsync(int submissionId, float grade);
}
