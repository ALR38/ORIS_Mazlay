namespace Domain.Entities;

public class Order
{
    public int OrderId { get; set; }        // Идентификатор заказа
    public int UserId { get; set; }         // Идентификатор пользователя
    public DateTime OrderDate { get; set; } // Дата заказа
    public decimal TotalPrice { get; set; } // Общая сумма заказа
    public string ShippingAddress { get; set; }  // Адрес доставки
    public string OrderStatus { get; set; }  // Статус заказа
    public string PaymentStatus { get; set; } // Статус оплаты

    // Навигационное свойство
    public User User { get; set; }          // Связь с пользователем
    public ICollection<OrderItem> OrderItems { get; set; }  // Связь с заказанными товарами
}
