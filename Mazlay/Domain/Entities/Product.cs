using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities;

public class Product : BaseEntity
{
    [Required, MaxLength(150)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Range(0, 999_999)]
    public decimal Price { get; set; }

    [Range(0, 100_000)]
    public int StockQuantity { get; set; }
    
    public string ImageMain { get; set; } = "no-photo.jpg";
    public string ImageAlt  { get; set; } = "no-photo.jpg";

    public int CategoryId  { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}