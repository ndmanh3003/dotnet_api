
namespace dotnet.Http.Middlewares;

public class RawBodyMiddleware
{
    private readonly RequestDelegate _next;

    public RawBodyMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.ContentType?.Contains("application/json") == true)
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Items["RawBody"] = body;
            context.Request.Body.Position = 0;
        }

        await _next(context);
    }
}