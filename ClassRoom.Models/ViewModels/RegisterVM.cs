using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;



namespace ClassRoom.Models.ViewModels;

public class RegisterVM
{
	[Required]
	[StringLength(50)]
	[Display(Name = "Last Name")]
	public string LastName { get; set; }
	[Required]
	[StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
	[Display(Name = "First Name")]
	public string FirstMidName { get; set; }

	[Required]
	[EmailAddress]
	public string Email { get; set; }



	[Required]
	[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
	[DataType(DataType.Password)]
	[Display(Name = "Password")]
	public string Password { get; set; }

	[DataType(DataType.Password)]
	[Display(Name = "Confirm password")]
	[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
	public string ConfirmPassword { get; set; }

	public IEnumerable<SelectListItem>? RoleList { get; set; }
	[Display(Name = "Role")]
	public string RoleSelected { get; set; }
}