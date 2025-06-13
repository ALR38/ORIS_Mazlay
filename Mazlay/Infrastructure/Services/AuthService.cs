
using Domain.Entities;
using Infrastructure.Constants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Infrastructure.Identity;

namespace Application.Services;

/// <summary>Регистрация пользователей.</summary>
public interface IAuthService
{
    Task<Result> RegisterAsync(RegisterDto dto);
}

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser>    _users;
    private readonly RoleManager<IdentityRole<Guid>> _roles;

    public AuthService(
        UserManager<ApplicationUser>    users,
        RoleManager<IdentityRole<Guid>> roles)
    {
        _users = users;
        _roles = roles;
    }

    public async Task<Result> RegisterAsync(RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email    = dto.Email
        };

        var create = await _users.CreateAsync(user, dto.Password);
        if (!create.Succeeded)
            return Result.Failure(create.Errors.Select(e => e.Description));

        // Гарантируем, что все роли есть.
        foreach (var role in AppRoles.All)
            if (!await _roles.RoleExistsAsync(role))
                await _roles.CreateAsync(new IdentityRole<Guid>(role));

        await _users.AddToRoleAsync(user, AppRoles.User);
        return Result.Success();
    }
}