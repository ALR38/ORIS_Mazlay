using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Common;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Constants;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

/// <inheritdoc />
public sealed class AuthService : IAuthService
{
    private readonly SignInManager<ApplicationUser>      _signIn;
    private readonly UserManager<ApplicationUser>        _users;
    private readonly RoleManager<IdentityRole<Guid>>     _roles;

    public AuthService(
        SignInManager<ApplicationUser>      signIn,
        UserManager<ApplicationUser>        users,
        RoleManager<IdentityRole<Guid>>     roles)
    {
        _signIn = signIn;
        _users  = users;
        _roles  = roles;
    }

    public async Task<bool> LoginAsync(string email, string password, bool remember)
    {
        var res = await _signIn.PasswordSignInAsync(email, password, remember, lockoutOnFailure: false);
        return res.Succeeded;
    }

    public async Task LogoutAsync() => await _signIn.SignOutAsync();

    public async Task<Result> RegisterAsync(RegisterDto dto)
    {
        var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email };

        var create = await _users.CreateAsync(user, dto.Password);
        if (!create.Succeeded)
            return Result.Failure(create.Errors.Select(e => e.Description).ToArray());

        // гарантируем наличие ролей
        foreach (var role in AppRoles.All)
            if (!await _roles.RoleExistsAsync(role))
                await _roles.CreateAsync(new IdentityRole<Guid>(role));

        await _users.AddToRoleAsync(user, AppRoles.User);
        return Result.Success();
    }
}