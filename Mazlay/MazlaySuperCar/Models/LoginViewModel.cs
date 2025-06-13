namespace MazlaySuperCar.Models;

public class LoginViewModel
{
    public string Email     { get; set; } = string.Empty;
    public string Password  { get; set; } = string.Empty;
    public bool   Remember  { get; set; }
    public string? ReturnUrl { get; set; }
}