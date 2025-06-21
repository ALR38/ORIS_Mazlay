using System.Threading.Tasks;
using Application.Common;
using Application.DTOs;

namespace Application.Abstractions;

/// <summary>
/// Авторизация, регистрация, выход пользователя.
/// </summary>
public interface IAuthService
{
    Task<bool>  LoginAsync   (string email, string password, bool remember);
    Task        LogoutAsync  ();
    Task<Result> RegisterAsync(RegisterDto dto);
}
