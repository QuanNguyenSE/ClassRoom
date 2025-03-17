namespace ClassRoom.Web.Services.IServices;

public interface IEnrollmentService
{
	Task<bool> EnrollStudentAsync(int courseId, string studentId);
}
