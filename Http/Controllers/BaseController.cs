using Microsoft.AspNetCore.Mvc;
using dotnet.Repositories;
using dotnet.Exceptions;
using dotnet.Http.Responses;
using dotnet.Common;
using dotnet.Http.Requests;
using dotnet.Models;

namespace dotnet.Http.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class BaseController<TEntity, TDto, TIndexDto, TIndexReq, TCreateReq, TUpdateReq>(
    BaseRepository<TEntity> repo
) : ControllerBase
    where TEntity : BaseEntity
    where TIndexReq : BaseIndexRequest
    where TCreateReq : class
    where TUpdateReq : class
    where TDto : BaseDto
    where TIndexDto : BaseDto
{
    protected readonly BaseRepository<TEntity> _repo = repo;

    [HttpGet]
    public virtual async Task<IActionResult> Index(
        [FromQuery] TIndexReq req,
        CancellationToken ct)
    {
        var (data, paginate) = await _repo.IndexAsync(req.To<TIndexReq>(), null);
        var items = data.To<List<TIndexDto>>();
        return ApiResponse.Ok(new { items, paginate });
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Detail(string id, CancellationToken ct)
    {
        var detail = await _repo.DetailAsync(ParseId(id), null);
        return ApiResponse.Ok(detail.To<TDto>());
    }

    [HttpPost]
    public virtual async Task<IActionResult> Store(
        [FromBody] TCreateReq req,
        CancellationToken ct)
    {
        var created = await _repo.StoreAsync(req.To<TEntity>());
        return ApiResponse.Ok(created.To<TDto>());
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(
        string id,
        [FromBody] TUpdateReq req,
        CancellationToken ct)
    {
        var data = await req.ToPartialRequest(Request);
        var updated = await _repo.UpdateAsync(ParseId(id), data);
        return ApiResponse.Ok(updated.To<TDto>());
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        await _repo.DeleteAsync(ParseId(id));
        return ApiResponse.Ok<object?>(null);
    }

    protected static int ParseId(string id)
    {
        if (int.TryParse(id, out var value))
            return value;

        throw new ApiException(400, "Invalid ID");
    }
}
