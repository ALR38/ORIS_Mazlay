using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity;

/// <summary>Первоначальные пользователи/роли.</summary>
public static class IdentitySeeder
{
    private static readonly (string Email, string Role)[] _seed =
    {
        ("admin@mail.com",   "Admin"),
        ("manager@mail.com", "Manager"),
        ("user@mail.com",    "User")
    };

    public static async Task SeedAsync(IServiceProvider sp)
    {
        var roleMgr = sp.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        foreach (var role in _seed.Select(s => s.Role).Distinct())
            if (!await roleMgr.RoleExistsAsync(role))
                await roleMgr.CreateAsync(new IdentityRole<Guid>(role));

        var userMgr = sp.GetRequiredService<UserManager<ApplicationUser>>();
        foreach (var (email, role) in _seed)
        {
            if (await userMgr.FindByEmailAsync(email) != null) continue;

            var u = new ApplicationUser
            {
                UserName       = email,
                Email          = email,
                EmailConfirmed = true
            };
            await userMgr.CreateAsync(u, "P@ssw0rd1!");
            await userMgr.AddToRoleAsync(u, role);
        }
    }
}