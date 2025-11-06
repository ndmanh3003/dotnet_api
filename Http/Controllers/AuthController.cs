using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using dotnet.Models;
using dotnet.Http.Responses;
using dotnet.Http.Requests.Auth;
using dotnet.Http.Responses.User;
using dotnet.Services;
using dotnet.Common;

namespace dotnet.Http.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(
    GoogleService googleService,
    AuthService authService,
    JwtService jwtService,
    IConfiguration configuration) : ControllerBase
{
    private readonly GoogleService _googleService = googleService;
    private readonly AuthService _authService = authService;
    private readonly JwtService _jwtService = jwtService;
    private readonly IConfiguration _configuration = configuration;

    [HttpPost("callback")]
    public async Task<IActionResult> Callback([FromBody] CallbackRequest request)
    {
        var redirectPath = request.RedirectUri.TrimStart('/');
        var redirectUri = $"{_configuration["Client:BaseUrl"]!.TrimEnd('/')}/{redirectPath}";

        var tokenResponse = await _googleService.ExchangeCodeForTokenAsync(request.Code, redirectUri);
        var userInfo = await _googleService.GetUserInfoAsync(tokenResponse.AccessToken);

        var user = await _authService.ProcessGoogleUserAsync(userInfo);
        var token = _jwtService.GenerateToken(user);

        return ApiResponse.Ok(new { token, user = user.To<UserDto>() });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return ApiResponse.Ok((HttpContext.Items["User"] as User)!.To<UserDto>());
    }
}