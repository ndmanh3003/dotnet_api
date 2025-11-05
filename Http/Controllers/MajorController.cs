using Microsoft.AspNetCore.Mvc;
using dotnet.Common.Validation;
using dotnet.Enums;
using dotnet.Http.Requests.Major;
using dotnet.Http.Responses.Major;
using dotnet.Models;
using dotnet.Repositories;

namespace dotnet.Http.Controllers;

[OnlyActions("Index", "Detail", "Store", "Update")]
public class MajorController(MajorRepository repo)
        : BaseController<Major, MajorDto, MajorIndexDto, IndexRequest, CreateRequest, UpdateRequest>(repo)
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
}
