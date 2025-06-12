// Program.cs
using Infrastructure.Data;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ─────────────────────  SERVICES  ─────────────────────

// MVC + Razor
builder.Services.AddControllersWithViews();

// EF Core  (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// ASP.NET Identity (int ключи)
builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(opt =>
    {
        opt.Password.RequireNonAlphanumeric = false;
        opt.User.RequireUniqueEmail          = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Настройка cookie-авторизации
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath        = "/Account/Login";
    opt.AccessDeniedPath = "/Account/Denied";
});

// -----------------------------------------------------------------------------
var app = builder.Build();

// ─────────────────────  PIPELINE  ─────────────────────

if (!app.Environment.IsDevelopment())
{
    // своя страница 500
    app.UseExceptionHandler("/Error/500");
    app.UseHsts();
}

app.UseHttpsRedirection();

// 1.  статика (wwwroot/…)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 2.  «красивые» страницы 404 / 403 / …
app.UseStatusCodePagesWithReExecute("/{0}");   // вызовет /404, /403 …

// 3.  основные маршруты
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();