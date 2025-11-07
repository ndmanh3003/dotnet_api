using dotnet.Models;

namespace dotnet.Repositories;

public class StudentRepository : BaseRepository<Student>
{
    public StudentRepository(ApplicationDbContext context) : base(context)
    {
        _searchableFields = ["Name", "Code"];
        _orderableFields = ["Id", "Name", "Year", "Code"];
    }

    public override async Task<Student> DetailAsync(int id, string[]? includes = null)
    {
        return await base.DetailAsync(id, includes ?? ["Major"]);
    }

    public override async Task<(List<Student> Items, Paginate Paginate)> IndexAsync<TCondition>(TCondition options, params string[]? includes)
    {
        return await base.IndexAsync(options, includes ?? ["Major"]);
    }
}