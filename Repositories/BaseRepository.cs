using dotnet.Common;
using dotnet.Exceptions;
using dotnet.Http.Requests;
using dotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace dotnet.Repositories;

public record Paginate(
    int Total,
    int CurrentPage,
    int PerPage
)
{
    public int TotalPages => (int)Math.Ceiling((double)Total / PerPage);
}

public abstract class BaseRepository<TEntity> : IServiceRegistration
    where TEntity : BaseEntity
{
    protected readonly DbContext _context;
    protected readonly DbSet<TEntity> _dbSet;
    protected string[] _searchableFields = ["Name"];
    protected string[] _orderableFields = ["Id"];

    protected BaseRepository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<TEntity> DetailAsync(int id)
        => await _dbSet.FindAsync(id)
           ?? throw new ApiException(404, $"{typeof(TEntity).Name} not found");

    public virtual async Task<TEntity> StoreAsync(TEntity entity)
    {
        var entry = await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public virtual async Task<TEntity> UpdateAsync(int id, Dictionary<string, object?> attributes)
    {
        var existing = await DetailAsync(id);
        _context.Entry(existing).CurrentValues.SetValues(attributes);
        await _context.SaveChangesAsync();
        return existing;
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await DetailAsync(id);
        entity.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public virtual async Task<(List<TEntity> Items, Paginate Paginate)> IndexAsync<TCondition>(TCondition options)
            where TCondition : BaseIndexRequest
    {
        var query = _dbSet.AsNoTracking().AsQueryable();

        query = ApplyKeywordFilter(query, options);
        query = ApplyConditions(query, options);
        query = ApplyOrdering(query, options);

        var total = await query.CountAsync();

        query = ApplyPaging(query, options);
        var data = await query.ToListAsync();
        var paginate = new Paginate(total, options.Page, options.Limit);

        return (data, paginate);
    }

    protected virtual IQueryable<TEntity> ApplyConditions<TCondition>(IQueryable<TEntity> query, TCondition? condition)
    {
        return query;
    }

    protected virtual IQueryable<TEntity> ApplyKeywordFilter(IQueryable<TEntity> query, BaseIndexRequest options)
    {
        if (string.IsNullOrWhiteSpace(options.Keyword))
            return query;

        var kw = options.Keyword.Trim().ToLower();
        var expr = string.Join(" || ", _searchableFields.Select(f => $"{f}.ToLower().Contains(@0)"));
        return query.Where(expr, kw);
    }

    protected virtual IQueryable<TEntity> ApplyOrdering(IQueryable<TEntity> query, BaseIndexRequest options)
    {
        var orderCol = string.IsNullOrWhiteSpace(options.OrderCol)
            || !_orderableFields.Contains(options.OrderCol, StringComparer.OrdinalIgnoreCase)
            ? "Id"
            : options.OrderCol;

        var dir = options.OrderDir.Equals("desc", StringComparison.OrdinalIgnoreCase)
                ? "descending"
                : "ascending";

        return query.OrderBy($"{orderCol} {dir}");
    }

    protected virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, BaseIndexRequest options)
    {
        var page = options.Page <= 0 ? 1 : options.Page;
        var limit = options.Limit <= 0 || options.Limit >= 100 ? 20 : options.Limit;

        return query.Skip((page - 1) * limit).Take(limit);
    }
}
