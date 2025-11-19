using dotnet.Http.Responses;
using dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Repositories;

public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context)
{
    public async Task<User?> FindByEmailAsync(string email, bool throwNotFound = false)
    {
        var user = await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null && throwNotFound)
        {
            ApiResponse.Fail("User not found", status: 404);
        }
        return user;
    }
}