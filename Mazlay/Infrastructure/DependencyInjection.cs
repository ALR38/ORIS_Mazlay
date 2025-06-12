using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection s, IConfiguration cfg)
    {
        // SQL Server - строка "Default"
        s.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseSqlServer(cfg.GetConnectionString("Default")));

        // Identity
        s.AddIdentity<ApplicationUser, IdentityRole<int>>(opt =>
            {
                opt.Password.RequireDigit           = true;
                opt.Password.RequiredLength         = 6;
                opt.Password.RequireUppercase       = false;
                opt.User.RequireUniqueEmail         = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // текущий пользователь
        s.AddHttpContextAccessor();
        s.AddScoped<ICurrentUser, CurrentUser>();

        return s;
    }
}