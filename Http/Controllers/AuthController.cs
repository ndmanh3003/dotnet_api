using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using dotnet.Repositories;
using dotnet.Enums;
using dotnet.Models;
using dotnet.Http.Responses;
using Microsoft.AspNetCore.Authorization;
using dotnet.Common;
using dotnet.Http.Responses.User;
using System.Security.Claims;

namespace dotnet.Http.Controllers;

[ApiController]
// [OnlyActions(nameof(Login))]
[Route("api/[controller]")]
public class AuthController(UserRepository userRepo, StudentRepository studentRepo) : ControllerBase
{
    private readonly string[] _adminEmails = ["ndmanh3003@gmail.com", "giaovu2@gmail.com"];
    private readonly UserRepository _userRepo = userRepo;
    private readonly StudentRepository _studentRepo = studentRepo;

    [HttpGet("login")]
    public async Task<IActionResult> Login()
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Url.Action("Login", "Auth", null, Request.Scheme)
            }, GoogleDefaults.AuthenticationScheme);

        var email = User.FindFirstValue("email")!;
        var user = await _userRepo.FindByEmailAsync(email);

        var isStudent = !_adminEmails.Contains(email);

        if (user is null)
        {
            user = await _userRepo.StoreAsync(new User
            {
                Email = email,
                GoogleId = User.FindFirstValue("sub")!,
                FullName = User.FindFirstValue("name")!,
                Name = User.FindFirstValue("given_name")!,
                Role = isStudent ? Role.Student : Role.Admin,
                Picture = User.FindFirstValue("picture")!
            });

            if (isStudent)
            {
                await _studentRepo.StoreAsync(new Student
                {
                    UserId = user.Id,
                    Name = User.FindFirstValue("name")!,
                    Code = null,
                    Year = null,
                    MajorCode = null,
                    Gender = null
                });
            }
        }

        return ApiResponse.Ok(user!.To<UserIndexDto>());
    }

    // [EnumAuthorize(Role.Student)]
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return ApiResponse.Ok((HttpContext.Items["User"] as User)!.To<UserDto>());
    }
}
