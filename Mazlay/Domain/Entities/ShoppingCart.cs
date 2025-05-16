namespace Domain.Entities;

public class ShoppingCart
{
    public int CartId { get; set; }          // Идентификатор корзины
    public int UserId { get; set; }          // Идентификатор пользователя
    public DateTime CreatedAt { get; set; }  // Дата создания корзины

    // Навигационное свойство
    public User User { get; set; }           // Связь с пользователем
    public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
}
