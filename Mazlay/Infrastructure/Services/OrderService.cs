using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public sealed class OrderService : IOrderService
{
    private readonly ICartService         _cart;
    private readonly ApplicationDbContext _db;

    public OrderService(ICartService cart, ApplicationDbContext db)
    {
        _cart = cart;
        _db   = db;
    }

    public async Task<int> PlaceOrderAsync(Guid userId)
    {
        var lines = await _cart.GetLinesAsync();
        if (lines.Count == 0) throw new InvalidOperationException("cart empty");

        var order = new Order
        {
            UserId  = userId,
            CreatedUtc = DateTime.UtcNow,
            Items   = lines.Select(l => new OrderItem
            {
                ProductId = l.ProductId,
                Price     = l.Price,
                Quantity  = l.Quantity
            }).ToList(),
            Total = lines.Sum(l => l.Price * l.Quantity)
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        await _cart.ClearAsync();
        return order.Id;
    }

    public async Task<IReadOnlyList<Order>> GetMyOrdersAsync(Guid userId)
    {
        var list = await _db.Orders
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedUtc)
            .ToListAsync();
        return list;
    }

    public async Task<IReadOnlyList<Order>> GetAllAsync()
    {
        var list = await _db.Orders
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .OrderByDescending(o => o.CreatedUtc)
            .ToListAsync();
        return list;
    }
}