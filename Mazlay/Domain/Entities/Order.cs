using System;
using System.Collections.Generic;

namespace Domain.Entities;

public sealed class Order
{
    public int      Id          { get; set; }
    public Guid     UserId      { get; set; }
    public DateTime CreatedUtc  { get; set; }  

    public decimal  Total       { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    /* навигация */
    public ApplicationUser User { get; set; } = null!;
}