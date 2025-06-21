// Application/Interfaces/IWishlistService.cs
using Application.DTOs;

namespace Application.Interfaces;

public interface IWishlistService
{
    Task ToggleAsync(int productId);
    Task ClearAsync();
    Task<IReadOnlyList<WishlistItemDto>> GetItemsAsync();
}