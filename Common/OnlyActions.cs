using dotnet.Common.Validation;
using dotnet.Http.Responses;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace dotnet.Common;

public class OnlyActionsFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var controllerType = context.Controller.GetType();
        var onlyAttr = controllerType.GetCustomAttribute<OnlyActionsAttribute>();

        if (onlyAttr is not null)
        {
            var actionName = context.ActionDescriptor.RouteValues["action"] ?? string.Empty;
            if (!onlyAttr.Allows(actionName))
            {
                ApiResponse.Fail("This action is not allowed.", status: 403);
            }
        }

        await next();
    }
}