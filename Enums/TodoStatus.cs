using System.ComponentModel.DataAnnotations;

namespace dotnet.Enums;

public enum TodoStatus
{
    [Display(Name = "Đang thực hiện")]
    InProgress,

    [Display(Name = "Hoàn thành")]
    Completed
}