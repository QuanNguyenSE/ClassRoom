using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models.ViewModels;

public class EditRoleVM
{
	public string Id { get; set; }

	[Required]
	[Display(Name = "Role Name")]
	public string Name { get; set; }
}
