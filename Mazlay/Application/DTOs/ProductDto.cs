namespace Application.DTOs;

/// <summary>Мини-DTO для вывода карточки товара.</summary>
public class ProductDto
{
    public int    Id         { get; set; }          // первичный ключ
    public string Name       { get; set; } = "";    // название
    public decimal Price     { get; set; }          // цена

    /// <summary>Имя файла главной картинки (product1.jpg).</summary>
    public string ImageMain  { get; set; } = "no-photo.jpg";

    /// <summary>Имя файла второй картинки (при ховере).</summary>
    public string ImageAlt   { get; set; } = "no-photo.jpg";
}