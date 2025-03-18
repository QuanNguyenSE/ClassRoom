using ClassRoom.Models.ViewModels;

namespace ClassRoom.Web.Services.IServices;

public interface ISubmissionService
{
	Task<SubmissionViewModel?> GetSubmissionFormAsync(int assignmentId, string studentId);
	Task<bool> SubmitAssignmentAsync(SubmissionCreateModel model);
	bool HasSubmittedAssignment(string studentId, int assignmentId);

	Task<SubmissionDetailViewModel?> GetSubmissionDetailAsync(int submissionId);
	Task<bool> UpdateSubmissionAsync(SubmissionUpdateModel model);
	Task<List<SubmissionUpdateViewModel>> GetSubmissionsByAssignmentAsync(int assignmentId);
}
