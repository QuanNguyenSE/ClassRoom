using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClassRoom.DataAccess.Data;
using ClassRoom.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;

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

		// ✅ Hiển thị tất cả các khóa học, có tìm kiếm và lọc theo khóa học đã đăng ký
		public async Task<IActionResult> Index(string search, bool showRegistered = false)
		{
			var user = await _userManager.GetUserAsync(User);
			var courses = _db.Courses.Include(c => c.Students).AsQueryable();

			if (!string.IsNullOrEmpty(search))
			{
				courses = courses.Where(c => c.Title.Contains(search));
			}

			if (showRegistered && user != null)
			{
				courses = courses.Where(c => c.Students.Contains(user));
			}

			return View(await courses.ToListAsync());
		}

		// ✅ Đăng ký khóa học
		public async Task<IActionResult> Register(int courseId)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null) return RedirectToAction("Login", "Account");

			var course = await _db.Courses.Include(c => c.Students)
										  .FirstOrDefaultAsync(c => c.Id == courseId);
			if (course == null) return NotFound();

			if (!course.Students.Contains(user))
			{
				course.Students.Add(user);
				await _db.SaveChangesAsync();
			}

			return RedirectToAction("Index");
		}

		// GET: Course/Details/5
		public async Task<IActionResult> Details(int? id)
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
