namespace dotnet.Http.Requests.Student;

using System.ComponentModel.DataAnnotations;
using dotnet.Common.Validation;
using dotnet.Enums;

public class UpdateRequest
{
    public int? Year { get; set; }

    [StringLength(20)]
    [Exists("majors", "code")]
    public string? MajorCode { get; set; }

    [ValidEnum<Gender>]
    public Gender Gender { get; set; }
}

