// Infrastructure/Services/WishlistService.cs
using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public sealed class WishlistService : IWishlistService
{
    private const string KEY = "maz.wish";
    private readonly IHttpContextAccessor _ctx;
    private readonly ApplicationDbContext _db;

    public WishlistService(IHttpContextAccessor ctx, ApplicationDbContext db)
    {
        _ctx = ctx;
        _db  = db;
    }

    private ISession Ses => _ctx.HttpContext!.Session;

    public async Task ToggleAsync(int productId)
    {
        var list = Ses.Get<List<WishlistItemDto>>(KEY) ?? [];

        int idx = list.FindIndex(i => i.ProductId == productId);
        if (idx >= 0)
            list.RemoveAt(idx);
        else
        {
            var p = await _db.Products
                .AsNoTracking()
                .SingleAsync(x => x.Id == productId);
            list.Add(new(p.Id, p.Name, p.ImageMain, p.Price));
        }

        Ses.Set(KEY, list);
    }

    public Task ClearAsync()
    {
        Ses.Remove(KEY);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<WishlistItemDto>> GetItemsAsync() =>
        Task.FromResult((IReadOnlyList<WishlistItemDto>)
            (Ses.Get<List<WishlistItemDto>>(KEY) ?? []));
}