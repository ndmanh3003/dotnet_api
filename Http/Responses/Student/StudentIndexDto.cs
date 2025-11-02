using dotnet.Http.Responses.User;

namespace dotnet.Http.Responses.Student;

public class StudentIndexDto : BaseDto
{
    public string StudentId { get; set; } = string.Empty;
}
