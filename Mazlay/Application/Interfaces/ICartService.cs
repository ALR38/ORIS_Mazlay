// Application/Interfaces/ICartService.cs
using Application.DTOs;

namespace Application.Interfaces;

public interface ICartService
{
    Task AddAsync(int productId, int qty = 1);
    Task RemoveAsync(int productId);
    Task UpdateQtyAsync(int productId, int qty);
    Task ClearAsync();
    Task<IReadOnlyList<CartLineDto>> GetLinesAsync();
}