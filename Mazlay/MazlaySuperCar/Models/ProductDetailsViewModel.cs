using Domain.Entities;

namespace MazlaySuperCar.Models;

public class ProductDetailsViewModel
{
    public required Product Product { get; init; }

    public int? PrevId { get; init; }
    public int? NextId { get; init; }
}