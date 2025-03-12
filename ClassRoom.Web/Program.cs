using ClassRoom.DataAccess.Data;
using ClassRoom.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>();

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

app.Run();
