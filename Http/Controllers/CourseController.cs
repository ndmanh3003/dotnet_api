using dotnet.Common.Validation;
using dotnet.Enums;
using dotnet.Http.Requests.Course;
using dotnet.Http.Responses.Course;
using dotnet.Models;
using dotnet.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace dotnet.Http.Controllers;

public class CourseController(CourseRepository repo)
        : BaseController<Course, CourseDto, CourseIndexDto, IndexRequest, CreateRequest, UpdateRequest>(repo)
{
    [EnumAuthorize(Role.Admin)]
    public override async Task<IActionResult> Store([FromBody] CreateRequest req, CancellationToken ct)
    {
        return await base.Store(req, ct);
    }

    [EnumAuthorize(Role.Admin)]
    public override async Task<IActionResult> Update(string id, [FromBody] UpdateRequest req, CancellationToken ct)
    {
        return await base.Update(id, req, ct);
    }

    [EnumAuthorize(Role.Admin)]
    public override async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        return await base.Delete(id, ct);
    }
}
