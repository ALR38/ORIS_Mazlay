namespace Domain.Entities;

public class Category
{
    public int CategoryId { get; set; }    // Идентификатор категории
    public string Name { get; set; }       // Название категории
    public string Description { get; set; } // Описание категории

    // Навигационное свойство
    public ICollection<Product> Products { get; set; }
}
