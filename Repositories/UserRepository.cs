using dotnet.Http.Responses;
using dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Repositories;

public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context)
{
    public async Task<User?> FindByEmailAsync(string email, bool throwNotFound = false)
    {
        var user = await _dbSet
            .Include(u => u.Student!)
                .ThenInclude(s => s.Major)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null && throwNotFound)
            ApiResponse.Fail("User not found", status: 404);

        return user;
    }

    protected override IQueryable<User> ApplyIncludes(IQueryable<User> query, string[]? includes)
    {
        if (includes == null || includes.Length == 0)
            return query;

        if (includes.Contains("withStudent"))
            return query.Include(u => u.Student!)
                .ThenInclude(s => s.Major);

        return base.ApplyIncludes(query, includes);
    }
}