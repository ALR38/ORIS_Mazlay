// Infrastructure/Services/CartService.cs
using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public sealed class CartService : ICartService
{
    private const string KEY = "maz.cart";
    private readonly IHttpContextAccessor _ctx;
    private readonly ApplicationDbContext _db;

    public CartService(IHttpContextAccessor ctx, ApplicationDbContext db)
    {
        _ctx = ctx;
        _db  = db;
    }

    private ISession Ses => _ctx.HttpContext!.Session;

    public async Task AddAsync(int productId, int qty = 1)
    {
        var list = Ses.Get<List<CartLineDto>>(KEY) ?? [];

        int idx = list.FindIndex(l => l.ProductId == productId);
        if (idx < 0)
        {
            var p = await _db.Products
                             .AsNoTracking()
                             .SingleAsync(x => x.Id == productId);

            list.Add(new(p.Id, p.Name, p.ImageMain, p.Price, qty));
        }
        else
        {
            var l = list[idx];
            list[idx] = l with { Quantity = l.Quantity + qty };
        }

        Ses.Set(KEY, list);
    }

    public Task RemoveAsync(int productId)
    {
        var list = Ses.Get<List<CartLineDto>>(KEY) ?? [];
        list.RemoveAll(l => l.ProductId == productId);
        Ses.Set(KEY, list);
        return Task.CompletedTask;
    }

    public Task UpdateQtyAsync(int productId, int qty)
    {
        var list = Ses.Get<List<CartLineDto>>(KEY) ?? [];
        int idx  = list.FindIndex(l => l.ProductId == productId);
        if (idx >= 0) list[idx] = list[idx] with { Quantity = qty };
        Ses.Set(KEY, list);
        return Task.CompletedTask;
    }

    public Task ClearAsync()
    {
        Ses.Remove(KEY);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<CartLineDto>> GetLinesAsync() =>
        Task.FromResult((IReadOnlyList<CartLineDto>)
            (Ses.Get<List<CartLineDto>>(KEY) ?? []));
}
