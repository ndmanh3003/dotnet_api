using dotnet.Enums;

namespace dotnet.Http.Responses.User;

public class UserDto : BaseDto
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public Role Role { get; set; }
    public string Picture { get; set; } = string.Empty;
}
