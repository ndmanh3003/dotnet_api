using dotnet.Enums;
using dotnet.Exceptions;
using dotnet.Http.Requests.Todo;

namespace dotnet.Repositories;

public class TodoRepository(ApplicationDbContext context) : BaseRepository<Models.Todo>(context)
{
    private int _currentUserId = 0;

    public void SetCurrentUserId(int userId)
    {
        _currentUserId = userId;
    }

    protected override IQueryable<Models.Todo> ApplyConditions<TCondition>(
        IQueryable<Models.Todo> query,
        TCondition? condition)
    where TCondition : class
    {
        if (condition is not IndexRequest req)
            return base.ApplyConditions(query, condition);

        query = query.Where(t => t.UserId == _currentUserId);

        if (req.Status is not null)
        {
            query = query.Where(t => t.Status == req.Status);
        }

        return query;
    }

    public override async Task<Models.Todo> DetailAsync(int id, params string[]? includes)
    {
        var entity = await base.DetailAsync(id, includes);
        return entity.UserId == _currentUserId ? entity : throw new ApiException(404, "Todo not found");
    }

    public override async Task<Models.Todo> StoreAsync(Models.Todo entity)
    {
        entity.UserId = _currentUserId;
        return await base.StoreAsync(entity);
    }

}

