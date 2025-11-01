namespace dotnet.Exceptions;

public class ApiException : Exception
{
    public int StatusCode { get; }
    public object Body { get; }

    public ApiException(int statusCode = 400, string message = "Bad Request", IEnumerable<object>? errors = null)
        : base(message)
    {
        StatusCode = statusCode;
        Body = new
        {
            status = statusCode,
            message,
            errors = errors ?? []
        };
    }
}
