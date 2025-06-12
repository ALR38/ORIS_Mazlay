using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services;
public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _ctx;
    public CurrentUser(IHttpContextAccessor ctx) => _ctx = ctx;
    private ClaimsPrincipal? Principal => _ctx.HttpContext?.User;

    public int?   Id    => int.TryParse(Principal?.FindFirstValue(ClaimTypes.NameIdentifier), out var i) ? i : null;
    public string? Email => Principal?.FindFirstValue(ClaimTypes.Email);
    public bool IsInRole(string role) => Principal?.IsInRole(role) ?? false;
}