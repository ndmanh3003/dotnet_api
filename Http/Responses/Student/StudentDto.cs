using dotnet.Http.Responses.User;

namespace dotnet.Http.Responses.Student;

public class StudentDto : BaseDto
{
    public string StudentId { get; set; } = string.Empty;

    public UserIndexDto? User { get; set; }
}
