using System.ComponentModel.DataAnnotations;
using dotnet.Enums;

namespace dotnet.Http.Requests.Todo;

public class CreateRequest
{
    [Required]
    [MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }

    [EnumDataType(typeof(TodoStatus))]
    public TodoStatus Status { get; set; } = TodoStatus.InProgress;
}

