using dotnet.Common;
using dotnet.Enums;
using dotnet.Http.Responses.Auth;
using dotnet.Models;
using dotnet.Repositories;

namespace dotnet.Services;

public class AuthService(UserRepository userRepository, StudentRepository studentRepository, IConfiguration configuration) : IServiceRegistration
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly StudentRepository _studentRepository = studentRepository;
    private readonly IConfiguration _configuration = configuration;

    public async Task<User> ProcessGoogleUserAsync(GoogleUserInfo userInfo)
    {
        var user = await _userRepository.FindByEmailAsync(userInfo.Email);

        if (user is not null)
            return user;

        var adminEmails = _configuration.GetSection("AdminEmails").Get<string[]>()!;
        var isStudent = !adminEmails.Contains(userInfo.Email);

        user = await _userRepository.StoreAsync(new User
        {
            Email = userInfo.Email,
            GoogleId = userInfo.Id,
            FullName = userInfo.Name,
            Name = userInfo.GivenName,
            Role = isStudent ? Role.Student : Role.Admin,
            Picture = userInfo.Picture
        });

        if (isStudent)
        {
            await _studentRepository.StoreAsync(new Student
            {
                UserId = user.Id,
                Name = userInfo.Name,
            });
        }

        return user;
    }
}

