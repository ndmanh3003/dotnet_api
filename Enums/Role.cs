using System.ComponentModel.DataAnnotations;

namespace dotnet.Enums;

public enum Role
{
    [Display(Name = "Quản trị viên")]
    Admin,

    [Display(Name = "Người dùng")]
    User,
}