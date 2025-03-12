using ClassRoom.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClassRoom.DataAccess.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public DbSet<ApplicationUser> ApplicationUser { get; set; }
		public ApplicationDbContext(DbContextOptions options)
			: base(options)
		{
		}
		override protected void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
