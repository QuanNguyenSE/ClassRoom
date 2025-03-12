using Microsoft.AspNetCore.Mvc;

namespace ClassRoom.Web.Controllers
{
	public class UserController : Controller
	{
		public UserController()
		{
			
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}
