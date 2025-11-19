using dotnet.Enums;

namespace dotnet.Http.Responses.Todo;

public class TodoDto : BaseDto
{
    public string Title { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public TodoStatus Status { get; set; }
}

