using ClassRoom.DataAccess.Data;
using ClassRoom.Models;
using ClassRoom.Web.Hubs;
using ClassRoom.Web.Services;
using ClassRoom.Web.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();


builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireDigit = false; // Không bắt buộc có số
	options.Password.RequiredLength = 1;  // Chỉ cần 1 ký tự là đủ
	options.Password.RequireLowercase = false; // Không cần chữ thường
	options.Password.RequireUppercase = false; // Không cần chữ hoa
	options.Password.RequireNonAlphanumeric = false; // Không cần ký tự đặc biệt
});

builder.Services.ConfigureApplicationCookie(opt =>
{
	opt.AccessDeniedPath = new PathString("/Account/NoAccess");
});

builder.Services.AddSignalR();

builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chatHub");




// Seed data
using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	context.Database.Migrate();

	if (!context.Courses.Any())
	{
		context.Courses.AddRange(
		new Course { Title = "Web design & development courses for beginner", Description = "Learn C# from scratch", Level = CourseLevel.Beginner, ImageUrl = "/images/ca96e87d-e7cd-420a-8fe1-ee9b23b10982_course-2.jpg", MinStudentToOpenClass = 5 },
		new Course { Title = "Web design & development courses for beginner", Description = "Build web apps using ASP.NET Core", Level = CourseLevel.Intermediate, ImageUrl = "/images/ca96e87d-e7cd-420a-8fe1-ee9b23b10982_course-2.jpg", MinStudentToOpenClass = 10 },
		new Course { Title = "Web design & development courses for beginner", Description = "Master relational databases", Level = CourseLevel.Advanced, ImageUrl = "/images/ca96e87d-e7cd-420a-8fe1-ee9b23b10982_course-2.jpg", MinStudentToOpenClass = 8 },
		new Course { Title = "Web design & development courses for beginner", Description = "Learn C# from scratch", Level = CourseLevel.Beginner, ImageUrl = "/images/ca96e87d-e7cd-420a-8fe1-ee9b23b10982_course-2.jpg", MinStudentToOpenClass = 5 },
		new Course { Title = "Web design & development courses for beginner", Description = "Build web apps using ASP.NET Core", Level = CourseLevel.Intermediate, ImageUrl = "/images/ca96e87d-e7cd-420a-8fe1-ee9b23b10982_course-2.jpg", MinStudentToOpenClass = 10 },
		new Course { Title = "Web design & development courses for beginner", Description = "Master relational databases", Level = CourseLevel.Advanced, ImageUrl = "/images/ca96e87d-e7cd-420a-8fe1-ee9b23b10982_course-2.jpg", MinStudentToOpenClass = 8 }
		);
		context.SaveChanges();
	}
}
app.Run();