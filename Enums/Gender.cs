using System.ComponentModel;

namespace dotnet.Enums;

public enum Gender
{
    [Description("Nam")]
    Male = 1,

    [Description("Nữ")]
    Female = 2,

    [Description("Khác")]
    Other = 9
}
