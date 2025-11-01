using dotnet.Enums.Course;

namespace dotnet.Http.Responses.Course;

public class CourseDto : BaseDto
{
    public string CourseId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Credits { get; set; }
    public CourseType Type { get; set; }
    public int TheoryHours { get; set; }
    public int PracticeHours { get; set; }
    public int ExerciseHours { get; set; }
}
