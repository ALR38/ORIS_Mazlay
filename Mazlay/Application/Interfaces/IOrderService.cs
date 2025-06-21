using Domain.Entities;

public interface IOrderService
{
    Task<int> PlaceOrderAsync(Guid userId);
    Task<IReadOnlyList<Order>> GetMyOrdersAsync(Guid userId);
    Task<IReadOnlyList<Order>> GetAllAsync();
}