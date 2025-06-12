using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Order : BaseEntity
{
    public int ApplicationUserId { get; set; }
    public ApplicationUser User  { get; set; } = null!;

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal  TotalPrice { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public OrderStatus   Status   { get; set; } = OrderStatus.Draft;
    public PaymentStatus PayState { get; set; } = PaymentStatus.Pending;

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}