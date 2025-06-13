using System.Security.Claims;

namespace MazlaySuperCar.Helpers;

public static class IdentityExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
        => int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
}