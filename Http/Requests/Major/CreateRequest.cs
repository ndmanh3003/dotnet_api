namespace dotnet.Http.Requests.Major;

using dotnet.Common.Validation;
using System.ComponentModel.DataAnnotations;

public class CreateRequest
{
    [Required, StringLength(20)]
    [Unique("majors", "code")]
    public string Code { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

