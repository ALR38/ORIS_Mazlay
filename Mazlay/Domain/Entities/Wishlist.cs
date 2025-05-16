namespace Domain.Entities;

public class Wishlist
{
    public int WishlistId { get; set; }     // Идентификатор списка желаемого
    public int UserId { get; set; }         // Идентификатор пользователя

    // Навигационное свойство
    public User User { get; set; }          // Связь с пользователем
    public ICollection<WishlistItem> WishlistItems { get; set; }
}
