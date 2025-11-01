using dotnet.Models;

namespace dotnet.Repositories;

public class CourseRepository(ApplicationDbContext context) : BaseRepository<Course>(context)
{
}