// Application/DTOs/WishlistItemDto.cs
namespace Application.DTOs;

public record WishlistItemDto(
    int     ProductId,
    string  Name,
    string  Image,
    decimal Price);