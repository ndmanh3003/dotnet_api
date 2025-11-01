namespace dotnet.Http.Requests;

public class BaseIndexRequest
{
    public string OrderCol { get; set; } = "Id";
    public string OrderDir { get; set; } = "desc";
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 20;
    public string Keyword { get; set; } = "";
}