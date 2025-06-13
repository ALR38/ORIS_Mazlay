using Domain.Entities;
using Infrastructure;
using Infrastructure.Hubs;             
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

/*─────────────  LOGGING  (Serilog)  ─────────────*/
builder.Host.UseSerilog((ctx, log) => log
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day));

/*─────────────  SERVICES  ─────────────*/
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

/*  ─── DbContext + Identity с Guid ───  */
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(opt =>
    {
        opt.Password.RequireNonAlphanumeric = false;
        opt.User.RequireUniqueEmail          = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

/*  Куки-авторизация  */
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath        = "/Login/Login";
    opt.AccessDeniedPath = "/Account/Denied";
});

/*─────────────  DI-контейнер из слоя Infrastructure  ─────────────*/
builder.Services.RegisterInfrastructure(builder.Configuration);
/* ↑ внутри уже есть AddSignalR, IAuthService, Mongo-контекст и др. */

/*─────────────  PIPELINE  ─────────────*/
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

/*  SignalR  */
app.MapHub<NotificationHub>("/hubs/notifications");

/*  MVC-маршрут  */
app.MapControllerRoute(
    name:   "default",
    pattern:"{controller=Home}/{action=Index}/{id?}");

app.Run();
