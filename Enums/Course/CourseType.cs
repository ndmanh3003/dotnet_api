using System.ComponentModel;

namespace dotnet.Enums.Course;

public enum CourseType
{
    [Description("Bắt buộc")]
    Compulsory = 1,

    [Description("Tự chọn")]
    Optional = 2
}