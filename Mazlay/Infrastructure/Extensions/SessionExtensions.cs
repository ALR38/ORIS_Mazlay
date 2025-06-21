// Infrastructure/Extensions/SessionExtensions.cs
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Extensions;

public static class SessionExtensions
{
    private static readonly JsonSerializerOptions _opts =
        new(JsonSerializerDefaults.Web);

    public static void Set<T>(this ISession s, string key, T value) =>
        s.SetString(key, JsonSerializer.Serialize(value, _opts));

    public static T? Get<T>(this ISession s, string key)
    {
        var json = s.GetString(key);
        return json is null ? default : JsonSerializer.Deserialize<T>(json, _opts);
    }
}