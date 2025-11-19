using dotnet.Common;
using dotnet.Enums;
using dotnet.Http.Responses.Auth;
using dotnet.Models;
using dotnet.Repositories;

namespace dotnet.Services;

public class AuthService(UserRepository userRepository) : IServiceRegistration
{
    private readonly UserRepository _userRepository = userRepository;
    public async Task<User> ProcessGoogleUserAsync(GoogleUserInfo userInfo)
    {
        var user = await _userRepository.FindByEmailAsync(userInfo.Email);

        if (user is not null)
            return user;

        user = await _userRepository.StoreAsync(new User
        {
            Email = userInfo.Email,
            GoogleId = userInfo.Id,
            FullName = userInfo.Name,
            Name = userInfo.GivenName,
            Picture = userInfo.Picture,
            Role = Role.User
        });

        return user;
    }
}

