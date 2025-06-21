// MazlaySuperCar/Models/CartPageViewModel.cs
using Application.DTOs;

namespace MazlaySuperCar.Models;

public sealed class CartPageViewModel
{
    public IReadOnlyList<CartLineDto> Lines { get; init; } = [];
    public decimal Subtotal                 { get; init; }
}