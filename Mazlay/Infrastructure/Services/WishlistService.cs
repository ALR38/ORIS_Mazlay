using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Infrastructure.Services;

/// <summary>Wish-list в Session (Mongo удалён).</summary>
public sealed class WishlistService : IWishlistService
{
    private readonly ISession _s;
    private const string Key = "WishIds";

    public WishlistService(IHttpContextAccessor ctx) =>
        _s = ctx.HttpContext!.Session;

    public Task<IList<int>> GetAsync(Guid _) =>
        Task.FromResult<IList<int>>(Read());

    public Task ToggleAsync(Guid _, int productId)
    {
        var list = Read();
        if (!list.Remove(productId)) list.Add(productId);
        Write(list);
        return Task.CompletedTask;
    }

    /*──────── helpers ────────*/
    List<int> Read()  => JsonSerializer.Deserialize<List<int>>(_s.GetString(Key) ?? "[]")!;
    void Write(List<int> d) => _s.SetString(Key, JsonSerializer.Serialize(d));
}