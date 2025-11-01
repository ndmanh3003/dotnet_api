using dotnet.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace dotnet.Http.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DropdownController() : ControllerBase
{
    private const int DefaultLimit = 50;

    private static readonly Dictionary<string, string[]> ModelMapping = new()
    {
        { "courses", [ "id", "name", "course_id", "credits" ] },
    };

    [HttpGet]
    public IActionResult Get([FromQuery] string enums, [FromQuery] string models, [FromQuery] int page = 1, [FromQuery] int? limit = null)
    {
        var response = new Dictionary<string, object?> { };

        if (!string.IsNullOrWhiteSpace(enums))
        {
            var enumDict = new Dictionary<string, object>();
            response["enum"] = enumDict;

            foreach (var key in enums.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                enumDict[key] = GetEnumValues(key);
            }
        }

        if (!string.IsNullOrWhiteSpace(models))
        {
            var modelDict = new Dictionary<string, object>();
            response["model"] = modelDict;

            foreach (var key in models.Split(',', StringSplitOptions.RemoveEmptyEntries))
                modelDict[key] = GetModelValues(key, page, limit ?? DefaultLimit);
        }

        return Ok(response);
    }

    private IEnumerable<object> GetModelValues(string modelKey, int page, int limit)
    {
        var key = modelKey.Trim().ToLower();

        if (!ModelMapping.TryGetValue(key, out var columns) || columns.Length < 2)
            return [];

        var _db = HttpContext.RequestServices.GetService<ApplicationDbContext>()!;

        try
        {
            var selectCols = new List<string>
                {
                    $"\"{columns[0]}\" AS \"value\"",
                    $"\"{columns[1]}\" AS \"label\""
                };
            selectCols.AddRange(columns.Skip(2).Select(c => $"\"{c}\""));

            if (page < 1) page = 1;
            var offset = (page - 1) * limit;
            var sql = $"SELECT {string.Join(", ", selectCols)} FROM \"{key}\" LIMIT {limit} OFFSET {offset}";


            using var conn = _db.Database.GetDbConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            using var reader = cmd.ExecuteReader();
            var rows = new List<Dictionary<string, object?>>();

            while (reader.Read())
            {
                var row = new Dictionary<string, object?>();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                rows.Add(row);
            }

            return rows;
        }
        catch
        {
            return [];
        }
    }

    private static IEnumerable<object> GetEnumValues(string key)
    {
        try
        {
            var parts = key.Split('.', StringSplitOptions.RemoveEmptyEntries);
            var enumName = ToPascalFromSnake(parts.Last());
            var nsParts = parts.Take(parts.Length - 1).Select(ToPascalFromSnake);
            string fullName = nsParts.Any()
                            ? $"dotnet.Enums.{string.Join(".", nsParts)}.{enumName}"
                            : $"dotnet.Enums.{enumName}";

            var enumType = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == fullName);

            if (enumType == null || !enumType.IsEnum)
                return [];

            return Enum.GetValues(enumType)
                .Cast<Enum>()
                .Select(e => new
                {
                    value = Convert.ToInt32(e),
                    label = e.GetType()
                        .GetField(e.ToString())?
                        .GetCustomAttribute<DescriptionAttribute>()?.Description ?? e.ToString()
                })
                .ToList();
        }
        catch
        {
            return [];
        }
    }

    private static string ToPascalFromSnake(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;
        input = input.ToLowerInvariant();
        var parts = Regex.Split(input, @"[_\-]+");
        return string.Concat(parts.Select(p =>
            p.Length == 0 ? "" : char.ToUpper(p[0]) + p.Substring(1)
        ));
    }
}
