using ClassRoom.Models.ViewModels;
using ClassRoom.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClassRoom.Web.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		// GET: Account/Register
		public IActionResult Register(string returnurl = null)
		{
			ViewData["ReturnUrl"] = returnurl;
			return View();
		}

		// POST: Account/Register
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(
			[Bind("LastName,FirstMidName,Email,Password,ConfirmPassword")] RegisterVM registerVM,
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
					await _signInManager.SignInAsync(user, isPersistent: false);
					return LocalRedirect(returnurl);
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
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
	}
}
