using dotnet.Http.Responses.Major;
using dotnet.Enums;

namespace dotnet.Http.Responses.Student;

public class StudentIndexDto : BaseDto
{
    public string? Code { get; set; }

    public string Name { get; set; } = string.Empty;

    public int? Year { get; set; }

    public MajorIndexDto? Major { get; set; }

    public Gender? Gender { get; set; }
}