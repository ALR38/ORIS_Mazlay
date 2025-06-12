namespace Application.Interfaces;

public interface ICurrentUser
{
    int?     Id         { get; }
    string?  Email      { get; }
    bool     IsInRole(string role);
}