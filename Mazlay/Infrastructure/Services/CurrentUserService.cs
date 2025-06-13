using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Infrastructure.Services;

/// <inheritdoc />
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _http;

    public CurrentUserService(IHttpContextAccessor http) => _http = http;

    public Guid Id
    {
        get
        {
            var raw = _http.HttpContext?.User?
                .FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(raw, out var guid) ? guid : Guid.Empty;
        }
    }

    public bool IsAuthenticated =>
        _http.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role) =>
        _http.HttpContext?.User?.IsInRole(role) ?? false;
}