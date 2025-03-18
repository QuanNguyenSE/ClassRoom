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
		public DbSet<ClassroomStudent> ClassroomStudents { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Assignment> Assignments { get; set; }
		public DbSet<Submission> Submissions { get; set; }



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
			modelBuilder.Entity<ClassroomStudent>().ToTable("ClassroomStudent");
			modelBuilder.Entity<Post>().ToTable("Post");
			modelBuilder.Entity<Assignment>().ToTable("Assignment");
			modelBuilder.Entity<Submission>().ToTable("Submission");


			modelBuilder.Entity<ClassroomStudent>()
				.HasOne(cs => cs.Classroom)
				.WithMany(c => c.ClassroomStudents)
				.HasForeignKey(cs => cs.ClassroomId)
				.OnDelete(DeleteBehavior.NoAction); // Ngăn chặn vòng lặp xóa

			modelBuilder.Entity<ClassroomStudent>()
				.HasOne(cs => cs.Student)
				.WithMany(s => s.ClassroomStudents)
				.HasForeignKey(cs => cs.StudentId)
				.OnDelete(DeleteBehavior.NoAction); // Ngăn chặn vòng lặp xóa

			modelBuilder.Entity<Assignment>()
			   .HasOne(a => a.Classroom)
			   .WithMany(c => c.Assignments)
			   .HasForeignKey(a => a.ClassroomId)
			   .OnDelete(DeleteBehavior.NoAction); // Hoặc .OnDelete(DeleteBehavior.SetNull)

			modelBuilder.Entity<Assignment>()
				.HasOne(a => a.CreatedBy)
				.WithMany()
				.HasForeignKey(a => a.CreatedById)
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Post>()
			   .HasOne(p => p.Classroom)
			   .WithMany(c => c.Posts)
			   .HasForeignKey(p => p.ClassroomId)
			   .OnDelete(DeleteBehavior.NoAction); // Hoặc .OnDelete(DeleteBehavior.SetNull)

			modelBuilder.Entity<Post>()
				.HasOne(p => p.CreatedBy)
				.WithMany()
				.HasForeignKey(p => p.CreatedById)
				.OnDelete(DeleteBehavior.NoAction);

		}
	}
}
