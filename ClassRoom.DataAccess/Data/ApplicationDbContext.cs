using ClassRoom.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClassRoom.DataAccess.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public DbSet<ApplicationUser> ApplicationUser { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<Enrollment> Enrollments { get; set; }
		public DbSet<Classroom> Classrooms { get; set; }

		public ApplicationDbContext(DbContextOptions options)
			: base(options)
		{
		}
		override protected void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Course>().ToTable("Course");
			modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
			modelBuilder.Entity<Classroom>().ToTable("Classroom");

		}
	}
}
