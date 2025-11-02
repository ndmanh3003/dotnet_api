using dotnet.Http.Responses;
using dotnet.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace dotnet.Http.Middlewares;

public class CurrentUserMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var hasAuthorize = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>() != null;

        if (!hasAuthorize)
        {
            await _next(context);
            return;
        }

        if (context.User.Identity?.IsAuthenticated ?? false)
        {
            var email = context.User.FindFirstValue("email");
            if (email == null)
                ApiResponse.Fail("Email claim is missing", status: 400);

            var userRepo = context.RequestServices.GetRequiredService<UserRepository>();
            var user = await userRepo.FindByEmailAsync(email!, true);
            context.Items["User"] = user;
        }

        await _next(context);
    }
}
