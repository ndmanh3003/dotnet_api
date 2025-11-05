using System.ComponentModel.DataAnnotations;

namespace dotnet.Http.Requests.Major;

public class UpdateRequest
{
    [StringLength(100)]
    public string? Name { get; set; } = string.Empty;

    public bool? IsActive { get; set; } = null;
}

