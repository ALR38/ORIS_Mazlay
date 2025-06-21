// Program.cs

using System;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Hubs;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

/* ────────────────── LOGGING ────────────────── */
builder.Host.UseSerilog((ctx, log) => log
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day));

/* ────────────────── SERVICES ────────────────── */
builder.Services.AddControllersWithViews();

/* базовая инфраструктура (AutoMapper, e‑mail и т.д.) */
builder.Services.RegisterInfrastructure(builder.Configuration);

/* HttpContextAccessor нужен Cart/Wishlist‑сервисам */
builder.Services.AddHttpContextAccessor();

/* session‑based Cart / Wishlist / Order */
builder.Services.AddScoped<ICartService,     CartService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IOrderService,    OrderService>();

/* Session (cookie“.mazlay.session”) */
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(opt =>
{
    opt.Cookie.Name        = ".mazlay.session";
    opt.Cookie.IsEssential = true;

    /* ← вставить прямо здесь */
    opt.Cookie.SecurePolicy = CookieSecurePolicy.None;

    opt.IdleTimeout        = TimeSpan.FromDays(7);
});


/* PostgreSQL DbContext (миграции лежат в Persistence) */
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("Default"),
        b => b.MigrationsAssembly("Persistence")));

/* Identity + cookie‑auth */
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(opt =>
    {
        opt.Password.RequireNonAlphanumeric = false;
        opt.User.RequireUniqueEmail         = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath        = "/Login";
    opt.AccessDeniedPath = "/Account/Denied";
});

/* ────────────────── PIPELINE ────────────────── */
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/404");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();        // ← Session ДО auth/authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/{0}");

/* SignalR (notifications) */
app.MapHub<NotificationHub>("/hubs/notifications");

/* MVC */
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}");

app.Run();
