using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Infrastructure.Services;

/// <summary>Корзина, хранимая в Session (Mongo удалён).</summary>
public sealed class CartService : ICartService
{
    private readonly ISession _s;
    private readonly ApplicationDbContext _db;
    private const string Key = "CartLines";

    public CartService(IHttpContextAccessor ctx, ApplicationDbContext db)
    {
        _s  = ctx.HttpContext!.Session;
        _db = db;
    }

    /* вернуть строки */
    public Task<IReadOnlyList<CartLineDto>> GetAsync(Guid _)
        => Task.FromResult((IReadOnlyList<CartLineDto>)Read());

    /* добавить / увеличить qty */
    public async Task AddAsync(Guid _, int productId, int qty = 1)
    {
        var p = await _db.Products.FindAsync(productId);
        if (p is null) return;

        var list = Read();
        var idx  = list.FindIndex(l => l.ProductId == productId);

        if (idx < 0)
            list.Add(new CartLineDto(p.Id, p.Name, p.Price, qty));
        else
            list[idx] = list[idx] with { Qty = list[idx].Qty + qty };

        Write(list);
    }

    public Task RemoveAsync(Guid _, int productId)
    {
        var list = Read();
        list.RemoveAll(l => l.ProductId == productId);
        Write(list);
        return Task.CompletedTask;
    }

    public Task ClearAsync(Guid _) { _s.Remove(Key); return Task.CompletedTask; }

    /*──────── helpers ────────*/
    List<CartLineDto> Read() =>
        JsonSerializer.Deserialize<List<CartLineDto>>(_s.GetString(Key) ?? "[]")!;

    void Write(List<CartLineDto> data) =>
        _s.SetString(Key, JsonSerializer.Serialize(data));
}