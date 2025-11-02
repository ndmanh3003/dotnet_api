using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using dotnet.Repositories;
using dotnet.Enums;
using dotnet.Models;
using dotnet.Http.Responses;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using dotnet.Common;
using dotnet.Http.Responses.User;

namespace dotnet.Http.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ApplicationDbContext context, UserRepository userRepo) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;
    private readonly string[] _adminEmails = ["ndmanh3003@gmail.com", "giaovu2@gmail.com"];
    private readonly UserRepository _userRepo = userRepo;

    [HttpGet("login")]
    public async Task<IActionResult> Login()
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Url.Action("Login", "Auth", null, Request.Scheme)
            }, GoogleDefaults.AuthenticationScheme);

        var email = User.FindFirstValue("email")!;
        var user = await _context.Users
            .Include(u => u.Student)
            .FirstOrDefaultAsync(u => u.Email == email);

        user ??= await _userRepo.StoreAsync(new User
        {
            Email = email,
            GoogleId = User.FindFirstValue("sub")!,
            FullName = User.FindFirstValue("name")!,
            Name = User.FindFirstValue("given_name")!,
            Role = _adminEmails.Contains(email) ? Role.Admin : Role.Student,
            Picture = User.FindFirstValue("picture")!
        });

        return ApiResponse.Ok(user.To<UserIndexDto>());
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return ApiResponse.Ok((HttpContext.Items["User"] as User)!.To<UserDto>());
    }
}
