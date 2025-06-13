using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces;

public record CartLineDto(int ProductId, string Name, decimal Price, int Qty)
{
    public decimal Subtotal => Price * Qty;
}

public interface ICartService
{
    Task<IReadOnlyList<CartLineDto>> GetAsync(string userId);
    Task AddAsync   (string userId, int productId, int qty = 1);
    Task RemoveAsync(string userId, int productId);
    Task ClearAsync (string userId);
}