namespace Infrastructure.Identity;

/// <summary>Простая обёртка результата.</summary>
public record Result(bool Succeeded, IEnumerable<string> Errors)
{
    public static Result Success()                      => new(true, Enumerable.Empty<string>());
    public static Result Failure(IEnumerable<string> e) => new(false, e);
    public static Result Failure(string error)          => new(false, new[] { error });
}