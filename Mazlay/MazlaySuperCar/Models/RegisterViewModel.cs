using System.ComponentModel.DataAnnotations;

namespace MazlaySuperCar.Models;

/// <summary>Поля формы регистрации + простая валидация.</summary>
public class RegisterViewModel
{
    [Required(ErrorMessage = "E-mail обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный e-mail")]
    public string Email { get; init; } = null!;

    [Required, DataType(DataType.Password)]
    [MinLength(8,  ErrorMessage = "Минимум 8 символов")]
    [RegularExpression(@"^(?=.*\d)(?=.*[A-Za-z]).+$",
        ErrorMessage = "Пароль должен содержать буквы и цифры")]
    public string Password { get; init; } = null!;

    [Required, DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; init; } = null!;
}