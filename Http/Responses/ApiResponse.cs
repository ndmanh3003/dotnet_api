using Microsoft.AspNetCore.Mvc;
using dotnet.Exceptions;

namespace dotnet.Http.Responses;

public static class ApiResponse
{
    public static IActionResult Ok<T>(T data, string message = "Success")
        => new ObjectResult(new { status = 200, message, data }) { StatusCode = 200 };

    public static void Fail(string message = "Bad Request", List<string>? errors = null, int status = 400)
        => throw new ApiException(status, message, errors);
}
