namespace Application.DTOs;

/// <summary>
/// Данные, приходящие с формы регистрации.
/// </summary>
public sealed record RegisterDto(string Email, string Password);