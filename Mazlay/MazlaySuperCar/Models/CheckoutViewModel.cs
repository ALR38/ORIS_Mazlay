using Application.Interfaces;
using System.Collections.Generic;

namespace MazlaySuperCar.Models;

public class CheckoutViewModel
{
    public IReadOnlyList<CartLineDto> Lines { get; set; } = new List<CartLineDto>();
}