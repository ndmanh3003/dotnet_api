using dotnet.Http.Requests.Major;
using dotnet.Models;

namespace dotnet.Repositories;

public class MajorRepository : BaseRepository<Major>
{
    public MajorRepository(ApplicationDbContext context) : base(context)
    {
        _searchableFields = ["Name", "Code"];
        _orderableFields = ["Id", "Name", "Code"];
    }

    protected override IQueryable<Major> ApplyConditions<TCondition>(
    IQueryable<Major> query,
    TCondition condition)
    where TCondition : class
    {
        if (condition is not IndexRequest req)
            return base.ApplyConditions(query, condition);

        if (req.IsActive is not null)
            query = query.Where(m => m.IsActive == req.IsActive);

        return base.ApplyConditions(query, condition);
    }

}