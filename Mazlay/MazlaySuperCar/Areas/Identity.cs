using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity;

public static class IdentitySeeder
{
    private static readonly (string Email,string Role)[] _seed =
    {
        ("admin@mail.com",   "Admin"),
        ("manager@mail.com", "Manager"),
        ("user@mail.com",    "User")
    };

    public static async Task SeedAsync(IServiceProvider sp)
    {
        var roleMgr = sp.GetRequiredService<RoleManager<IdentityRole<int>>>();
        foreach (var r in _seed.Select(s => s.Role).Distinct())
            if (!await roleMgr.RoleExistsAsync(r))
                await roleMgr.CreateAsync(new IdentityRole<int>(r));

        var userMgr = sp.GetRequiredService<UserManager<ApplicationUser>>();
        foreach (var (email,role) in _seed)
        {
            if (await userMgr.FindByEmailAsync(email) != null) continue;

            var u = new ApplicationUser { UserName=email, Email=email, EmailConfirmed=true };
            await userMgr.CreateAsync(u,"P@ssw0rd1!");
            await userMgr.AddToRoleAsync(u, role);
        }
    }
}