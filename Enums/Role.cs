using System.ComponentModel;

namespace dotnet.Enums;

public enum Role
{
    [Description("Học sinh")]
    Student = 1,

    [Description("Giáo vụ")]
    Admin = 2,

    [Description("Giảng viên")]
    Lecturer = 3
}