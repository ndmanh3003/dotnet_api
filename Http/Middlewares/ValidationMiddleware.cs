using System.Text.Json;
using dotnet.Exceptions;

namespace dotnet.Http.Middlewares;

public class ValidationMiddleware(RequestDelegate next, ILogger<ValidationMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ValidationMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        var originalBody = context.Response.Body;
        await using var memStream = new MemoryStream();
        context.Response.Body = memStream;

        try
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
            {
                memStream.Seek(0, SeekOrigin.Begin);
                var body = await new StreamReader(memStream).ReadToEndAsync();

                try
                {
                    using var doc = JsonDocument.Parse(body);

                    if (
                        doc.RootElement.TryGetProperty("title", out var titleProp)
                        && titleProp.GetString() == "One or more validation errors occurred."
                        && doc.RootElement.TryGetProperty("errors", out var errorsProp)
                    )
                    {
                        var errors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(errorsProp.GetRawText())
                            ?? new Dictionary<string, string[]>();
                        context.Response.Body = originalBody;

                        throw new ApiException(
                            statusCode: 422,
                            message: "Validation failed",
                            errors: errors.Select(e => new { field = e.Key, messages = e.Value })
                        );
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "Validation middleware parse error");
                }
            }

            memStream.Seek(0, SeekOrigin.Begin);
            await memStream.CopyToAsync(originalBody);
        }
        finally
        {
            // luôn đảm bảo khôi phục stream gốc, kể cả khi có exception
            context.Response.Body = originalBody;
        }
    }
}
