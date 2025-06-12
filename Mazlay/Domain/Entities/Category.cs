using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities;

public class Category : BaseEntity
{
    [Required, MaxLength(120)]
    public string Name { get; set; } = null!;

    [MaxLength(500)]
    public string? Description { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}