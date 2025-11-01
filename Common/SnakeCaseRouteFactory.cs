using Humanizer;

namespace dotnet.Common;

public class SnakeCaseRouteFactory : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
        => value?.ToString()?.Underscore();
}