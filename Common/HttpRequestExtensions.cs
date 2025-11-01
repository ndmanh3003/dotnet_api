using System.Text.Json;
using System.Reflection;

namespace dotnet.Common;

public static class HttpRequestExtensions
{
    public static async Task<Dictionary<string, object?>> ToPartialRequest<T>(this T obj, HttpRequest request)
    {
        if (!request.HttpContext.Items.TryGetValue("RawBody", out var rawObj) || rawObj is not string body)
            throw new InvalidOperationException("Raw JSON body not found");

        using var doc = JsonDocument.Parse(body);
        var fields = doc.RootElement.EnumerateObject()
            .Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var dict = new Dictionary<string, object?>();
        foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (fields.Contains(prop.Name))
                dict[prop.Name] = prop.GetValue(obj);
        }

        return dict;
    }
}