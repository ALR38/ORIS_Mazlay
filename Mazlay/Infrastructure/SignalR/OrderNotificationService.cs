using Application.DTOs;
using Infrastructure.Constants;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Infrastructure.SignalR;

/// <summary>Отправляет события о заказах в хаб.</summary>
public interface IOrderNotificationService
{
    Task NotifyOrderPlacedAsync(OrderDto order);
}

public class OrderNotificationService : IOrderNotificationService
{
    private readonly IHubContext<NotificationHub> _hub;

    public OrderNotificationService(IHubContext<NotificationHub> hub) => _hub = hub;

    public Task NotifyOrderPlacedAsync(OrderDto order) =>
        _hub.Clients
            .Groups(HubGroups.Admins, HubGroups.Managers)
            .SendAsync("OrderPlaced", order);
}