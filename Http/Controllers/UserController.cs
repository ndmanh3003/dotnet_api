using Microsoft.AspNetCore.Authorization;
using dotnet.Http.Requests;
using dotnet.Http.Responses.User;
using dotnet.Models;
using dotnet.Repositories;
using dotnet.Common.Validation;

namespace dotnet.Http.Controllers;

[Authorize(Roles = "Admin")]
[OnlyActions("Index")]
public class UserController(UserRepository repo) : BaseController<
    User,
    UserDto,
    UserIndexDto,
    BaseIndexRequest,
    object,
    object
>(repo)
{
}

