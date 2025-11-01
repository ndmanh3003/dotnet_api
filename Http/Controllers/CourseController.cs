using dotnet.Http.Requests.Course;
using dotnet.Http.Responses.Course;
using dotnet.Models;
using dotnet.Repositories;

namespace dotnet.Http.Controllers;

public class CourseController(CourseRepository repo)
        : BaseController<Course, CourseDto, CourseIndexDto, IndexRequest, CreateRequest, UpdateRequest>(repo)
{
}
