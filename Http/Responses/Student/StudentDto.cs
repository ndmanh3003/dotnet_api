using dotnet.Http.Responses.User;
using dotnet.Http.Responses.Major;
using dotnet.Enums;
using System.Text.Json.Serialization;

namespace dotnet.Http.Responses.Student;

public class StudentDto : BaseDto
{
    public string? Code { get; set; }

    public string Name { get; set; } = string.Empty;

    public int? Year { get; set; }

    public MajorIndexDto? Major { get; set; }

    public Gender? Gender { get; set; }

    public UserIndexDto? User { get; set; }
}
