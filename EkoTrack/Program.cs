using System.Security.Claims;
using EkoTrack.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// SQL Server
var cs = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=LAPTOPDAYA\\SQLEXPRESS;Database=EcoTrack;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(cs));

builder.Services.AddControllersWithViews();

// Auth por COOKIES
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/Auth/Login";
        o.AccessDeniedPath = "/Auth/AccessDenied";
        o.ExpireTimeSpan = TimeSpan.FromHours(8);
        o.SlidingExpiration = true;
    });

// Autorización (roles por ClaimTypes.Role)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Ciudadano", p => p.RequireClaim(ClaimTypes.Role, "CIUDADANO"));
    options.AddPolicy("Recolector", p => p.RequireClaim(ClaimTypes.Role, "RECOLECTOR"));
    options.AddPolicy("AdminCentro", p => p.RequireClaim(ClaimTypes.Role, "ADMIN_CENTRO"));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // <- importante
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
