using dotnet.Enums.Course;
using dotnet.Http.Requests.Course;
using dotnet.Models;

namespace dotnet.Repositories;

public class CourseRepository : BaseRepository<Course>
{
    public CourseRepository(ApplicationDbContext context) : base(context)
    {
        _searchableFields = ["Name", "Code"];
        _orderableFields = ["Credits"];
    }

    protected override IQueryable<Course> ApplyConditions<TCondition>(
        IQueryable<Course> query,
        TCondition condition)
    where TCondition : class
    {
        if (condition is not IndexRequest req)
            return base.ApplyConditions(query, condition);

        if (!string.IsNullOrWhiteSpace(req.Type))
            if (Enum.TryParse<CourseType>(req.Type, true, out var parsedType))
            {
                query = query.Where(m => m.Type == parsedType);
            }
        return base.ApplyConditions(query, condition);
    }
}