using ClassRoom.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassRoom.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ChatRoomController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public ChatRoomController(ApplicationDbContext db)
		{
			_db = db;
		}

		//// Lấy danh sách room chat của một lớp học
		//[HttpGet("{classroomId}")]
		//public IActionResult GetRooms(int classroomId)
		//{
		//	var rooms = _db.ChatRooms.Where(r => r.ClassroomId == classroomId).ToList();
		//	return Ok(rooms);
		//}

		//// Tạo room chat mới
		//[HttpPost]
		//public async Task<IActionResult> CreateRoom([FromBody] ChatRoom model)
		//{
		//	if (string.IsNullOrEmpty(model.Name)) return BadRequest("Tên room không được trống");

		//	_db.ChatRooms.Add(model);
		//	await _db.SaveChangesAsync();
		//	return Ok(model);
		//}
	}
}
