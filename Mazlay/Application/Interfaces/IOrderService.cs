using Domain.Entities;

namespace Application.Interfaces;

public interface IOrderService
{
    Task<int> PlaceOrderAsync(Guid userId);
    Task<IReadOnlyList<Order>> GetMyOrdersAsync(Guid userId);
    Task<IReadOnlyList<Order>> GetAllAsync();
    Task<Order> GetOrderByIdAsync(int orderId);
}