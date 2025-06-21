using System.ComponentModel.DataAnnotations;

namespace MazlaySuperCar.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Введите e-mail")]
    [EmailAddress(ErrorMessage = "Некорректный e-mail")]
    public string Email { get; init; } = null!;

    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    public string Password { get; init; } = null!;

    public bool Remember { get; init; }
    public string? ReturnUrl { get; init; }
}