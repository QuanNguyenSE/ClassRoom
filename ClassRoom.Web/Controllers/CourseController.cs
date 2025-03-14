using ClassRoom.DataAccess.Data;
using ClassRoom.Models;
using ClassRoom.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClassRoom.Web.Controllers
{
	public class CourseController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public CourseController(ApplicationDbContext db,
			UserManager<ApplicationUser> userManager,
			IWebHostEnvironment webHostEnvironment)
		{
			_db = db;
			_userManager = userManager;
			_webHostEnvironment = webHostEnvironment;
		}


		public async Task<IActionResult> Index(string searchTerm)
		{
			var courses = _db.Courses
				.Include(c => c.Enrollments)
				.Select(c => new CourseVM
				{
					Id = c.Id,
					Title = c.Title,
					Duration = c.Duration,
					Level = c.Level,
					ImageUrl = c.ImageUrl,
					CreatedAt = c.CreatedAt,
					StudentCount = c.Enrollments.Count() // Số lượng học viên đã đăng ký
				});

			if (!string.IsNullOrEmpty(searchTerm))
			{
				courses = courses.Where(c => c.Title.Contains(searchTerm));
			}

			var courseList = await courses.ToListAsync();
			return View(courseList);
		}

		// Action đăng ký khóa học
		[HttpPost]
		public async Task<IActionResult> Register(int courseId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy ID user hiện tại
			if (userId == null)
			{
				return RedirectToAction("Login", "Account");
			}

			var course = await _db.Courses.FindAsync(courseId);
			if (course == null)
			{
				return NotFound();
			}

			// Kiểm tra xem user đã đăng ký khóa học này chưa
			var existingEnrollment = await _db.Enrollments
				.FirstOrDefaultAsync(e => e.StudentId == userId && e.CourseId == courseId);
			if (existingEnrollment != null)
			{
				TempData["error"] = "Bạn đã đăng ký khóa học này!";
				return RedirectToAction("Details", new { id = courseId });

			}

			// Thêm bản ghi mới vào bảng Enrollment
			var enrollment = new Enrollment
			{
				StudentId = userId,
				CourseId = courseId
			};
			_db.Enrollments.Add(enrollment);
			await _db.SaveChangesAsync();

			TempData["Success"] = "Đăng ký khóa học thành công!";
			return RedirectToAction("Index");
		}

		// GET: Course/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			var course = await _db.Courses
			.Where(c => c.Id == id)
			.Select(course => new CourseVM
			{
				Id = course.Id,
				Title = course.Title,
				Duration = course.Duration,
				Level = course.Level,
				ImageUrl = course.ImageUrl,
				CreatedAt = course.CreatedAt,
				StudentCount = _db.Enrollments.Count(e => e.CourseId == course.Id)
			})
			.FirstOrDefaultAsync();

			if (course == null)
			{
				return NotFound();
			}

			return View(course);
		}

		// GET: Course/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Course/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Course course, IFormFile? file)
		{
			if (ModelState.IsValid)
			{
				if (file != null)
				{
					// Lưu ảnh vào wwwroot/images/
					string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
					string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
					string filePath = Path.Combine(uploadsFolder, uniqueFileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await file.CopyToAsync(fileStream);
					}

					course.ImageUrl = "/images/" + uniqueFileName;
				}
				else
				{
					course.ImageUrl = "/images/default-course.png"; // Ảnh mặc định nếu không upload ảnh
				}

				_db.Courses.Add(course);
				await _db.SaveChangesAsync();

				return RedirectToAction("Index");
			}

			return View(course);
		}

		// GET: Course/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var course = await _db.Courses.FindAsync(id);
			if (course == null)
			{
				return NotFound();
			}
			return View(course);
		}

		// POST: Course/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Duration,Level,ImageUrl,CreatedAt")] Course course)
		{
			if (id != course.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_db.Update(course);
					await _db.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CourseExists(course.Id))
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
			return View(course);
		}

		// GET: Course/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var course = await _db.Courses
				.FirstOrDefaultAsync(m => m.Id == id);
			if (course == null)
			{
				return NotFound();
			}

			return View(course);
		}

		// POST: Course/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var course = await _db.Courses.FindAsync(id);
			if (course != null)
			{
				_db.Courses.Remove(course);
			}

			await _db.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool CourseExists(int id)
		{
			return _db.Courses.Any(e => e.Id == id);
		}
	}
}
