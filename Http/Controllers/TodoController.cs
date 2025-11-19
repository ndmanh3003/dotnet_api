using dotnet.Http.Requests.Todo;
using dotnet.Http.Responses.Todo;
using dotnet.Models;
using dotnet.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace dotnet.Http.Controllers;

[Authorize]
public class TodoController(TodoRepository repo) : BaseController<
    Todo,
    TodoDto,
    TodoIndexDto,
    IndexRequest,
    CreateRequest,
    UpdateRequest>(repo),
    IAsyncActionFilter
{

    private void SetCurrentUser()
    {
        var user = (HttpContext.Items["User"] as User)!;
        ((TodoRepository)_repo).SetCurrentUserId(user.Id);
    }

    [NonAction]
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        SetCurrentUser();
        await next();
    }
}

