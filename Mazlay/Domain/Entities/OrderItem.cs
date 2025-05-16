namespace Domain.Entities;

public class OrderItem
{
    public int OrderItemId { get; set; }    // Идентификатор элемента заказа
    public int OrderId { get; set; }        // Идентификатор заказа
    public int ProductId { get; set; }      // Идентификатор товара
    public int Quantity { get; set; }       // Количество товара
    public decimal Price { get; set; }      // Цена товара на момент покупки

    // Навигационные свойства
    public Order Order { get; set; }        // Связь с заказом
    public Product Product { get; set; }    // Связь с товаром
}
