using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<bool> LoginAsync   (string email, string password, bool remember);
    Task       LogoutAsync  ();
    Task<bool> RegisterAsync(RegisterDto dto);
}

public record RegisterDto(string Email, string Password);