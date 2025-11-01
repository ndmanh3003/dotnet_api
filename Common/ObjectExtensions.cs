using System.Text.Json;

namespace dotnet.Common;

public static class ObjectExtensions
{
    public static TDestination To<TDestination>(this object source)
    {
        var options = new JsonSerializerOptions
        {
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };
        return JsonSerializer.Deserialize<TDestination>(
            JsonSerializer.Serialize(source, options), options)!;
    }
}
