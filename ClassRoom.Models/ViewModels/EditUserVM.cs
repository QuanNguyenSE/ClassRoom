using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models.ViewModels;

public class EditUserVM
{
	public string Id { get; set; }

	[Required]
	[Display(Name = "Email")]
	public string Email { get; set; }

	[Required]
	[Display(Name = "First Name")]
	public string FirstMidName { get; set; }

	[Required]
	[Display(Name = "Last Name")]
	public string LastName { get; set; }

	public List<string> SelectedRoles { get; set; } = new();
	public List<RoleCheckboxItem> AllRoles { get; set; } = new();
}

public class RoleCheckboxItem
{
	public string RoleName { get; set; }
	public bool IsSelected { get; set; }
}