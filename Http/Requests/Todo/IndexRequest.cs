using dotnet.Enums;
using System.ComponentModel.DataAnnotations;

namespace dotnet.Http.Requests.Todo;

public class IndexRequest : BaseIndexRequest
{
    [EnumDataType(typeof(TodoStatus))]
    public TodoStatus? Status { get; set; }
}

