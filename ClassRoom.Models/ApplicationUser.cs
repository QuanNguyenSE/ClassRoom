using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models;

public class ApplicationUser : IdentityUser
{
	// Comon properties for all users
	[Required]
	[StringLength(50)]
	[Display(Name = "Last Name")]
	public string LastName { get; set; }
	[Required]
	[StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
	[Column("FirstName")]
	[Display(Name = "First Name")]
	public string FirstMidName { get; set; }

	[Display(Name = "Full Name")]
	public string FullName
	{
		get
		{
			return LastName + ", " + FirstMidName;
		}
	}

	// Properties for students
	[DataType(DataType.Date)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
	[Display(Name = "Enrollment Date")]
	public DateTime? EnrollmentDate { get; set; }

	//public ICollection<Enrollment>? Enrollments { get; set; }

	// Properties for instructors
	[DataType(DataType.Date)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
	[Display(Name = "Hire Date")]
	public DateTime? HireDate { get; set; }

	//public ICollection<CourseAssignment>? CourseAssignments { get; set; }
	//public OfficeAssignment? OfficeAssignment { get; set; }
}
