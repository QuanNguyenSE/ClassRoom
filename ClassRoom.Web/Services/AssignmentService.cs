using ClassRoom.DataAccess.Data;
using ClassRoom.Models;
using ClassRoom.Models.ViewModels;
using ClassRoom.Web.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace ClassRoom.Web.Services;

public class AssignmentService : IAssignmentService
{
	private readonly ApplicationDbContext _context;
	private readonly IWebHostEnvironment _env;

	public AssignmentService(ApplicationDbContext context, IWebHostEnvironment env)
	{
		_context = context;
		_env = env;
	}

	public async Task<List<Assignment>> GetAssignmentsAsync(int classroomId)
	{
		return await _context.Assignments
			.Where(a => a.ClassroomId == classroomId)
			.ToListAsync();
	}

	public async Task<Assignment?> GetAssignmentByIdAsync(int id)
	{
		return await _context.Assignments.FindAsync(id);
	}
	public async Task<AssignmentDetailViewModel?> GetAssignmentAsync(int id)
	{
		var assignment = await _context.Assignments
			.Include(a => a.CreatedBy)
			.FirstOrDefaultAsync(a => a.Id == id);

		if (assignment == null) return null;

		return new AssignmentDetailViewModel
		{
			Id = assignment.Id,
			Title = assignment.Title,
			Description = assignment.Description,
			FilePath = assignment.FilePath,
			DueDate = assignment.DueDate,
			CreatedBy = assignment.CreatedBy.FullName
		};
	}
	public async Task<bool> CreateAssignmentAsync(Assignment assignment, IFormFile? file)
	{
		if (file != null)
		{
			string filePath = await SaveFileAsync(file);
			assignment.FilePath = filePath;
		}

		_context.Assignments.Add(assignment);
		return await _context.SaveChangesAsync() > 0;
	}

	public async Task<bool> UpdateAssignmentAsync(Assignment assignment, IFormFile? file)
	{
		var existing = await _context.Assignments.FindAsync(assignment.Id);
		if (existing == null) return false;

		existing.Title = assignment.Title;
		existing.Description = assignment.Description;
		existing.DueDate = assignment.DueDate;

		if (file != null)
		{
			if (!string.IsNullOrEmpty(existing.FilePath))
			{
				DeleteFile(existing.FilePath);
			}
			existing.FilePath = await SaveFileAsync(file);
		}

		return await _context.SaveChangesAsync() > 0;
	}

	public async Task<bool> DeleteAssignmentAsync(int id)
	{
		var assignment = await _context.Assignments.FindAsync(id);
		if (assignment == null) return false;

		if (!string.IsNullOrEmpty(assignment.FilePath))
		{
			DeleteFile(assignment.FilePath);
		}

		_context.Assignments.Remove(assignment);
		return await _context.SaveChangesAsync() > 0;
	}

	private async Task<string> SaveFileAsync(IFormFile file)
	{
		string folderPath = Path.Combine(_env.WebRootPath, "docs");
		Directory.CreateDirectory(folderPath);

		string fileName = $"{Guid.NewGuid()}_{file.FileName}";
		string filePath = Path.Combine(folderPath, fileName);

		using (var stream = new FileStream(filePath, FileMode.Create))
		{
			await file.CopyToAsync(stream);
		}

		return $"/docs/{fileName}";
	}

	private void DeleteFile(string filePath)
	{
		string fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));
		if (File.Exists(fullPath))
		{
			File.Delete(fullPath);
		}
	}
}
