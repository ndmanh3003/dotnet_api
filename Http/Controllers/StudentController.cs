using Microsoft.AspNetCore.Mvc;
using dotnet.Common.Validation;
using dotnet.Enums;
using dotnet.Http.Requests.Student;
using dotnet.Http.Responses.Student;
using dotnet.Repositories;
using dotnet.Models;

namespace dotnet.Http.Controllers;

[OnlyActions("Index", "Detail", "Update")]
[EnumAuthorize(Role.Admin)]
public class StudentController(StudentRepository repo)
    : BaseController<Student, StudentDto, StudentIndexDto, IndexRequest, CreateRequest, UpdateRequest>(repo)
{
    public override async Task<IActionResult> Index([FromQuery] IndexRequest req, CancellationToken ct)
    {
        return await base.Index(req, ct);
    }

    public override async Task<IActionResult> Detail(string id, CancellationToken ct)
    {
        return await base.Detail(id, ct);
    }

    public override async Task<IActionResult> Update(string id, [FromBody] UpdateRequest req, CancellationToken ct)
    {
        return await base.Update(id, req, ct);
    }
}

