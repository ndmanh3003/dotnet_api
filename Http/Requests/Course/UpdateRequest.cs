using dotnet.Common.Validation;
using dotnet.Enums.Course;
using System.ComponentModel.DataAnnotations;

namespace dotnet.Http.Requests.Course;

public class UpdateRequest
{
    [StringLength(255)]
    public string? Name { get; set; } = string.Empty;

    [ValidEnum<CourseType>]
    public CourseType? Type { get; set; } = null;

    [Range(1, 10)]
    public int? Credits { get; set; }

    [Range(0, 100)]
    public int? TheoryHours { get; set; }

    [Range(0, 100)]
    public int? PracticeHours { get; set; }

    [Range(0, 100)]
    public int? ExerciseHours { get; set; }
}