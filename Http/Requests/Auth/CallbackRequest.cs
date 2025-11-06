namespace dotnet.Http.Requests.Auth;

public class CallbackRequest
{
    public string Code { get; set; } = string.Empty;
    public string RedirectUri { get; set; } = string.Empty;
}

