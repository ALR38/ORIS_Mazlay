namespace Domain.Entities;

public class ShoppingCartItem
{
    public int CartItemId { get; set; }     // Идентификатор элемента корзины
    public int CartId { get; set; }         // Идентификатор корзины
    public int ProductId { get; set; }      // Идентификатор товара
    public int Quantity { get; set; }       // Количество товара

    // Навигационные свойства
    public ShoppingCart Cart { get; set; }  // Связь с корзиной
    public Product Product { get; set; }    // Связь с товаром
}
