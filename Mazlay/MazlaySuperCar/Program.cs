// Program.cs
using Domain.Entities;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

/*───────────────  LOGGING  ───────────────*/
builder.Host.UseSerilog((ctx, log) => log
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day));

/*───────────────  SERVICES  ───────────────*/
builder.Services.AddControllersWithViews();

/*  Session (нужна для Cart / Wishlist)  */
builder.Services.AddDistributedMemoryCache();     
builder.Services.AddSession(opt =>
{
    opt.Cookie.Name        = ".mazlay.session";
    opt.Cookie.IsEssential = true;
    opt.IdleTimeout        = TimeSpan.FromDays(7);
});

/* DbContext + Identity (PostgreSQL, Guid) */
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("Default"),
        b => b.MigrationsAssembly("Persistence")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(opt =>
    {
        opt.Password.RequireNonAlphanumeric = false;
        opt.User.RequireUniqueEmail         = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

/* Куки-авторизация */
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath        = "/Login";
    opt.AccessDeniedPath = "/Account/Denied";
});

builder.Services.RegisterInfrastructure(builder.Configuration);

/*───────────────  PIPELINE  ───────────────*/
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/404");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();           
app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/{0}");

/* SignalR */
app.MapHub<NotificationHub>("/hubs/notifications");

/* MVC-маршрут */
app.MapControllerRoute(
    name:    "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
