namespace dotnet.Http.Requests.Course;

using dotnet.Common.Validation;
using dotnet.Enums.Course;
using System.ComponentModel.DataAnnotations;

public class CreateRequest
{
    [Required, StringLength(20)]
    [Unique("courses", "code")]
    public string Code { get; set; } = string.Empty;

    [Required, StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [ValidEnum<CourseType>]
    public CourseType Type { get; set; } = CourseType.Compulsory;

    [Required, Range(1, 10)]
    public int Credits { get; set; }

    [Required, Range(0, 100)]
    public int TheoryHours { get; set; }

    [Range(0, 100)]
    public int PracticeHours { get; set; }

    [Required, Range(0, 100)]
    public int ExerciseHours { get; set; }
}