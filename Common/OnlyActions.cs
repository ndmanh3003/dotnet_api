using dotnet.Http.Responses;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace dotnet.Common;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class OnlyActionsAttribute(params string[] actions) : Attribute
{
    public string[] Actions { get; } = actions ?? [];

    public bool Allows(string actionName)
    {
        if (Actions.Length == 0)
            return true;
        return Actions.Contains(actionName, StringComparer.OrdinalIgnoreCase);
    }
}

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