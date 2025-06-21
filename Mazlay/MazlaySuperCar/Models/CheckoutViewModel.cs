using Application.DTOs;

namespace MazlaySuperCar.Models;

public sealed class CheckoutViewModel
{
    public IReadOnlyList<CartLineDto> Lines { get; init; } = [];
    public decimal Total { get; init; }
}