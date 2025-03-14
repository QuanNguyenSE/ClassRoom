using ClassRoom.DataAccess.Data;
using ClassRoom.Models;
using ClassRoom.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClassRoom.Web.Controllers
{
	public class ClassroomController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly UserManager<ApplicationUser> _userManager;

		public ClassroomController(ApplicationDbContext context,
			UserManager<ApplicationUser> userManager)
		{
			_db = context;
			_userManager = userManager;
		}

		// GET: Classroom
		public async Task<IActionResult> Index()
		{
			var classrooms = await _db.Classrooms
				.Include(c => c.Course)
				.Include(c => c.Instructor)
				.ToListAsync();

			return View(classrooms);
		}

		// GET: Classroom/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var classroom = await _db.Classrooms
				.Include(c => c.Course)
				.Include(c => c.Instructor)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (classroom == null)
			{
				return NotFound();
			}

			return View(classroom);
		}

		// GET: Classroom/Create
		public async Task<IActionResult> Create(int courseId)
		{
			var course = await _db.Courses.Include(c => c.Enrollments)
										  .ThenInclude(e => e.Student)
										  .FirstOrDefaultAsync(c => c.Id == courseId);

			if (course == null)
			{
				return NotFound();
			}

			var instructors = await _userManager.GetUsersInRoleAsync("Instructor");

			var model = new ClassroomCreateVM
			{
				CourseId = courseId,
				Course = course,
				Students = course.Enrollments
								 .OrderBy(e => e.EnrollmentDate)
								 .Select(e => new StudentSelectionVM
								 {
									 StudentId = e.Student.Id,
									 FullName = e.Student.FullName,
									 Email = e.Student.Email,
									 Selected = true
								 }).ToList(),
				Instructors = instructors.Select(i => new SelectListItem
				{
					Value = i.Id,
					Text = i.FullName
				}).ToList()
			};

			return View(model);
		}

		// POST: Classroom/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		//[Authorize(Roles = "Staff")]

		public async Task<IActionResult> Create(ClassroomCreateVM model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var course = await _db.Courses.FindAsync(model.CourseId);
			if (course == null) return NotFound();

			var selectedStudents = model.Students.Where(s => s.Selected).ToList();
			if (!selectedStudents.Any())
			{
				TempData["Error"] = "Bạn cần chọn ít nhất một học viên!";
				return RedirectToAction("Create", new { courseId = model.Course.Id });
			}

			var classroom = new Classroom
			{
				Name = model.Name,
				CourseId = model.CourseId,
				InstructorId = model.InstructorId
			};

			_db.Classrooms.Add(classroom);
			await _db.SaveChangesAsync();

			foreach (var student in selectedStudents)
			{
				var user = await _db.Users.OfType<ApplicationUser>().FirstOrDefaultAsync(u => u.Id == student.StudentId);
				if (user != null)
				{
					classroom.Students.Add(user);
				}
			}

			await _db.SaveChangesAsync();

			//var enrollmentsToRemove = _db.Enrollments
			//							 .Where(e => e.CourseId == model.CourseId && selectedStudents.Any(s => s.StudentId == e.StudentId));

			//_db.Enrollments.RemoveRange(enrollmentsToRemove);
			//await _db.SaveChangesAsync();

			TempData["Success"] = "Lớp học đã được tạo thành công!";
			return RedirectToAction("Index");
		}



		// GET: Classroom/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var classroom = await _db.Classrooms.FindAsync(id);
			if (classroom == null)
			{
				return NotFound();
			}
			ViewData["CourseId"] = new SelectList(_db.Courses, "Id", "ImageUrl", classroom.CourseId);
			ViewData["InstructorId"] = new SelectList(_db.ApplicationUser, "Id", "Id", classroom.InstructorId);
			return View(classroom);
		}

		// POST: Classroom/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CourseId,InstructorId,CreatedDate")] Classroom classroom)
		{
			if (id != classroom.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_db.Update(classroom);
					await _db.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ClassroomExists(classroom.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["CourseId"] = new SelectList(_db.Courses, "Id", "ImageUrl", classroom.CourseId);
			ViewData["InstructorId"] = new SelectList(_db.ApplicationUser, "Id", "Id", classroom.InstructorId);
			return View(classroom);
		}

		// GET: Classroom/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var classroom = await _db.Classrooms
				.Include(c => c.Course)
				.Include(c => c.Instructor)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (classroom == null)
			{
				return NotFound();
			}

			return View(classroom);
		}

		// POST: Classroom/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var classroom = await _db.Classrooms.FindAsync(id);
			if (classroom != null)
			{
				_db.Classrooms.Remove(classroom);
			}

			await _db.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ClassroomExists(int id)
		{
			return _db.Classrooms.Any(e => e.Id == id);
		}
	}
}
