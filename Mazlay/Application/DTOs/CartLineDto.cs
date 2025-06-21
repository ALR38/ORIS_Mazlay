namespace Application.DTOs;

public record CartLineDto(
    int     ProductId,
    string  Name,
    string  Image,
    decimal Price,
    int     Quantity)
{
    public decimal LineTotal => Price * Quantity;
}