namespace Domain.Entities;

public class WishlistItem
{
    public int WishlistItemId { get; set; } // Идентификатор элемента списка желаемого
    public int WishlistId { get; set; }     // Идентификатор списка желаемого
    public int ProductId { get; set; }      // Идентификатор товара

    // Навигационные свойства
    public Wishlist Wishlist { get; set; }  // Связь с списком желаемого
    public Product Product { get; set; }    // Связь с товаром
}
