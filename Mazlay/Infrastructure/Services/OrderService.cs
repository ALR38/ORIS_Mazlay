using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;
using Infrastructure.Hubs;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext         _db;
    private readonly IHubContext<NotificationHub> _hub;

    public OrderService(ApplicationDbContext db, IHubContext<NotificationHub> hub)
        => (_db, _hub) = (db, hub);

    public async Task<int> CreateAsync(Guid userId, IReadOnlyList<CartLineDto> lines)
    {
        if (lines.Count == 0) return 0;

        var order = new Order
        {
            ApplicationUserId = userId,
            OrderDate         = DateTime.UtcNow,
            Status            = OrderStatus.Draft,
            PayState          = PaymentStatus.Pending,
            TotalPrice        = lines.Sum(l => l.Subtotal),
            ShippingAddress   = string.Empty,
            Items = lines.Select(l => new OrderItem
            {
                ProductId = l.ProductId,
                Quantity  = l.Qty,
                UnitPrice = l.Price
            }).ToList()
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        await _hub.Clients.Group("Admins")
            .SendAsync("OrderPlaced",
                new { id = order.Id, user = userId, total = order.TotalPrice });

        return order.Id;
    }
}