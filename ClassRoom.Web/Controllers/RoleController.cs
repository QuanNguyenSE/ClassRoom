using ClassRoom.Models;
using ClassRoom.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClassRoom.Web.Controllers
{
	[Authorize(Roles = "Admin")]
	public class RoleController : Controller
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<ApplicationUser> _userManager;

		public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}

		// GET: /Roles
		public IActionResult Index()
		{
			var roles = _roleManager.Roles.ToList();
			return View(roles);
		}

		// GET: /Roles/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: /Roles/Create
		[HttpPost]
		public async Task<IActionResult> Create(string roleName)
		{
			if (string.IsNullOrWhiteSpace(roleName))
			{
				ModelState.AddModelError("", "Role name is required");
				return View();
			}

			// Kiểm tra xem role đã tồn tại chưa
			var existingRole = await _roleManager.FindByNameAsync(roleName);
			if (existingRole != null)
			{
				ModelState.AddModelError("", "This role already exists.");
				return View();
			}

			var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
			if (result.Succeeded)
			{
				TempData["Success"] = "Role created successfully";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", "Failed to create role.");
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return NotFound();
			}

			var role = await _roleManager.FindByIdAsync(id);
			if (role == null)
			{
				return NotFound();
			}

			var model = new EditRoleVM
			{
				Id = role.Id,
				Name = role.Name
			};
			return View(model);
		}

		// Xử lý cập nhật role
		[HttpPost]
		public async Task<IActionResult> Edit(EditRoleVM model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var role = await _roleManager.FindByIdAsync(model.Id);
			if (role == null)
			{
				return NotFound();
			}

			var existingRole = await _roleManager.FindByNameAsync(model.Name);
			if (existingRole != null && existingRole.Id != model.Id)
			{
				ModelState.AddModelError("", "This role name already exists.");
				return View(model);
			}

			role.Name = model.Name;
			var result = await _roleManager.UpdateAsync(role);
			if (result.Succeeded)
			{
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", "Failed to update role.");
			return View(model);
		}

		// POST: /Roles/Delete/{id}
		[HttpPost]
		public async Task<IActionResult> Delete(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role == null)
			{
				return NotFound();
			}

			// Kiểm tra nếu có user nào đang được gán role này
			var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
			if (usersInRole.Any())
			{
				TempData["Error"] = "Cannot delete this role because there are users assigned to it.";
				return RedirectToAction("Index");
			}

			var result = await _roleManager.DeleteAsync(role);
			if (!result.Succeeded)
			{
				TempData["Error"] = "Failed to delete role.";
			}

			return RedirectToAction("Index");
		}

	}
}
