namespace dotnet.Http.Requests.Course;

using System.ComponentModel.DataAnnotations;

public class IndexRequest : BaseIndexRequest
{
    [StringLength(20)]
    public string Type { get; set; } = string.Empty;
}
