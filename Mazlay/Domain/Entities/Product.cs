using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities;

public class Product : BaseEntity
{
    [Required, MaxLength(150)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Range(0, 999999)]
    public decimal Price { get; set; }

    [Range(0, 100000)]
    public int StockQuantity { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}