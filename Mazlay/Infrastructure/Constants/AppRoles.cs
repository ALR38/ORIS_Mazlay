namespace Infrastructure.Constants;

/// <summary>Все роли в одном месте — никаких магических строк.</summary>
public static class AppRoles
{
    public const string User    = "User";
    public const string Manager = "Manager";
    public const string Admin   = "Admin";

    public static readonly string[] All = { User, Manager, Admin };
}