using ClassRoom.Models;
using ClassRoom.Models.ViewModels;
using ClassRoom.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClassRoom.Web.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;


		public AccountController(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
		}

		// GET: Account/Register
		public async Task<IActionResult> Register(string returnurl = null)
		{
			if (!_roleManager.RoleExistsAsync(SD.Admin).GetAwaiter().GetResult())
			{
				await _roleManager.CreateAsync(new IdentityRole(SD.Admin));
				await _roleManager.CreateAsync(new IdentityRole(SD.Instructor));
				await _roleManager.CreateAsync(new IdentityRole(SD.Student));
				await _roleManager.CreateAsync(new IdentityRole(SD.Staff));

			}

			//List<SelectListItem> listItems = new();
			//listItems.Add(new SelectListItem()
			//{
			//	Value = SD.Instructor,
			//	Text = SD.Instructor
			//});
			//listItems.Add(new SelectListItem()
			//{
			//	Value = SD.Student,
			//	Text = SD.Student
			//});

			ViewData["ReturnUrl"] = returnurl;

			RegisterVM registerVM = new()
			{
				RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
				{
					Text = i,
					Value = i
				})
			};

			return View(registerVM);
		}

		// POST: Account/Register
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterVM registerVM,
			string returnurl = null)
		{

			ViewData["ReturnUrl"] = returnurl;
			returnurl = returnurl ?? Url.Content("~/");
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser
				{
					UserName = registerVM.Email,
					Email = registerVM.Email,
					LastName = registerVM.LastName,
					FirstMidName = registerVM.FirstMidName
				};

				var result = await _userManager.CreateAsync(user, registerVM.Password);
				if (result.Succeeded)
				{
					if (registerVM.RoleSelected != null && registerVM.RoleSelected.Length > 0 && registerVM.RoleSelected == SD.Instructor)
					{
						await _userManager.AddToRoleAsync(user, SD.Instructor);
					}
					else if (registerVM.RoleSelected == SD.Staff)
					{
						await _userManager.AddToRoleAsync(user, SD.Staff);
					}
					else if (registerVM.RoleSelected == SD.Admin)
					{
						await _userManager.AddToRoleAsync(user, SD.Admin);
					}
					else
					{
						await _userManager.AddToRoleAsync(user, SD.Student);
					}
					await _signInManager.SignInAsync(user, isPersistent: false);
					return LocalRedirect(returnurl);
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			//List<SelectListItem> listItems = new();
			//listItems.Add(new SelectListItem()
			//{
			//	Value = SD.Instructor,
			//	Text = SD.Instructor
			//});
			//listItems.Add(new SelectListItem()
			//{
			//	Value = SD.Student,
			//	Text = SD.Student
			//});
			registerVM.RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
			{
				Text = i,
				Value = i
			});
			return View(registerVM);
		}

		// GET: Account/Login
		public IActionResult Login(string returnurl = null)
		{
			ViewData["ReturnUrl"] = returnurl;
			return View();
		}

		// POST: Account/Login
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LoginAsync(LoginVM loginVM, string returnurl = null)
		{
			ViewData["ReturnUrl"] = returnurl;
			returnurl = returnurl ?? Url.Content("~/");
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe,
					lockoutOnFailure: false);
				if (result.Succeeded)
				{
					return LocalRedirect(returnurl);
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
					return View(loginVM);
				}
			}
			return View(loginVM);
		}

		// POST: Account/LogOff
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LogOff()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult NoAccess()
		{
			return View();
		}

	}
}
