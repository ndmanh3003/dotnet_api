using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System.Globalization;

namespace dotnet.Common;

public class SnakeCaseQueryFactory : IValueProviderFactory
{
    public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
    {
        var query = context.ActionContext.HttpContext.Request.Query;

        var mapped = new Dictionary<string, StringValues>(StringComparer.OrdinalIgnoreCase);
        foreach (var kvp in query)
        {
            mapped[ToPascalCase(kvp.Key)] = kvp.Value;
        }

        var queryCollection = new QueryCollection(mapped);

        var provider = new QueryStringValueProvider(
            BindingSource.Query,
            queryCollection,
            CultureInfo.InvariantCulture
        );

        context.ValueProviders.Add(provider);
        return Task.CompletedTask;
    }

    private static string ToPascalCase(string key)
    {
        if (!key.Contains('_')) return key;
        return string.Join("", key
            .Split('_', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => char.ToUpperInvariant(s[0]) + s[1..]));
    }
}
