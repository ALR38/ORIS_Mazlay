using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces;

/// <summary>
/// Работа с корзиной пользователя.
/// </summary>
public interface ICartService
{
    /// <summary>Содержимое корзины.</summary>
    Task<IReadOnlyList<CartLineDto>> GetAsync(Guid userId);

    /// <summary>Добавить товар <paramref name="productId"/> в количестве <paramref name="qty"/>.</summary>
    Task AddAsync   (Guid userId, int productId, int qty = 1);

    Task RemoveAsync(Guid userId, int productId);

    Task ClearAsync (Guid userId);
}

/// <summary>Строка корзины.</summary>
public readonly record struct CartLineDto(
    int    ProductId,
    string Name,
    decimal Price,
    int    Qty)
{
    public decimal Subtotal => Price * Qty;
}