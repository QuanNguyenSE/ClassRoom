using ClassRoom.DataAccess.Data;
using ClassRoom.Models;
using ClassRoom.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassRoom.Web.Controllers
{
	public class UserController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public UserController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_db = db;
			_userManager = userManager;
			_roleManager = roleManager;
		}


		public async Task<IActionResult> Index()
		{
			var users = _userManager.Users.ToList();
			var userRolesList = new List<UserWithRolesVM>();

			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user);
				userRolesList.Add(new UserWithRolesVM
				{
					User = user,
					Roles = roles.ToList()
				});
			}
			return View(userRolesList);
		}
		public async Task<IActionResult> Edit(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			var userRoles = await _userManager.GetRolesAsync(user);
			var allRoles = _roleManager.Roles.Select(r => new RoleCheckboxItem
			{
				RoleName = r.Name,
				IsSelected = userRoles.Contains(r.Name)
			}).ToList();

			var model = new EditUserVM
			{
				Id = user.Id,
				Email = user.Email,
				FirstMidName = user.FirstMidName,
				LastName = user.LastName,
				SelectedRoles = userRoles.ToList(),
				AllRoles = allRoles
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditUserVM model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByIdAsync(model.Id);
			if (user == null)
			{
				return NotFound();
			}

			user.Email = model.Email;
			user.FirstMidName = model.FirstMidName;
			user.LastName = model.LastName;

			var userRoles = await _userManager.GetRolesAsync(user);
			var selectedRoles = model.SelectedRoles ?? new List<string>();

			// Cập nhật role
			await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
			await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

			await _userManager.UpdateAsync(user);
			return RedirectToAction(nameof(Index));
		}
	}
}
