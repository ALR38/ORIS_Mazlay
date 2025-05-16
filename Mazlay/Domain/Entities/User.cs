namespace Domain.Entities;

public class User
{
    public int UserId { get; set; }            // Идентификатор пользователя
    public string FirstName { get; set; }      // Имя
    public string LastName { get; set; }       // Фамилия
    public string Email { get; set; }          // Электронная почта
    public string PasswordHash { get; set; }   // Хэш пароля
    public string PhoneNumber { get; set; }    // Номер телефона
    public string Address { get; set; }        // Адрес
    public DateTime CreatedAt { get; set; }    // Дата регистрации
    public DateTime UpdatedAt { get; set; }    // Дата последнего обновления

    // Навигационные свойства
    public ICollection<Order> Orders { get; set; }
    public ICollection<Wishlist> Wishlists { get; set; }
    public ICollection<ShoppingCart> ShoppingCarts { get; set; }
}
