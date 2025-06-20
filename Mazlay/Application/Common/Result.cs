namespace Application.Common;

/// <summary>
/// Унифицированный результат операций (Success / Failure).
/// </summary>
public sealed record Result(bool Succeeded, IEnumerable<string> Errors)
{
    public static Result Success() => new(true, Array.Empty<string>());

    public static Result Failure(params string[] errors) =>
        new(false, errors);
}