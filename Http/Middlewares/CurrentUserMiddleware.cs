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

        if (!(context.User.Identity?.IsAuthenticated ?? false))
        {
            await _next(context);
            return;
        }

        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            ApiResponse.Fail("Unauthorized");
            return;
        }

        var userRepo = context.RequestServices.GetRequiredService<UserRepository>();
        var user = await userRepo.DetailAsync(userId, ["withStudent"]);

        context.Items["User"] = user;

        var identity = (ClaimsIdentity?)context.User.Identity;
        if (identity != null && !identity.HasClaim(c => c.Type == ClaimTypes.Role))
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));
            context.User = new ClaimsPrincipal(identity);
        }

        await _next(context);
    }
}
