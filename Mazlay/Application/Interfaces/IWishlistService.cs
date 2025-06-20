using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces;

/// <summary>
/// «Избранное» пользователя.
/// </summary>
public interface IWishlistService
{
    Task<IList<int>> GetAsync(Guid userId);

    /// <summary>Добавить/удалить товар из wish‑листа.</summary>
    Task ToggleAsync(Guid userId, int productId);
}