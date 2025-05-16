namespace Domain.Entities;

public class Product
{
    public int ProductId { get; set; }      // Идентификатор товара
    public string Name { get; set; }        // Название товара
    public string Description { get; set; } // Описание товара
    public decimal Price { get; set; }      // Цена товара
    public int StockQuantity { get; set; }  // Количество на складе
    public int CategoryId { get; set; }     // Идентификатор категории товара

    // Навигационное свойство
    public Category Category { get; set; }  // Связь с категорией

    public ICollection<OrderItem> OrderItems { get; set; }  // Заказы, в которые входит этот товар
    public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
    public ICollection<WishlistItem> WishlistItems { get; set; }
}
