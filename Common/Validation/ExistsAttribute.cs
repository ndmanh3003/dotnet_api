using System.ComponentModel.DataAnnotations;
using dotnet.Repositories;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Common.Validation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ExistsAttribute(string tableName, string columnName) : ValidationAttribute
{
    private readonly string _tableName = tableName;
    private readonly string _columnName = columnName;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        var dbContext = validationContext.GetService<ApplicationDbContext>();
        if (dbContext == null)
        {
            var serviceProvider = validationContext.GetService<IServiceProvider>();
            dbContext = serviceProvider?.GetService<ApplicationDbContext>();
        }

        if (dbContext == null)
            throw new InvalidOperationException("ApplicationDbContext is not available in validation context.");

        var sql = $"SELECT 1 FROM `{_tableName}` WHERE `{_columnName}` = @p0 LIMIT 1";
        var exists = dbContext.Database
            .SqlQueryRaw<int>(sql, value)
            .Any();

        if (!exists)
        {
            var msg = ErrorMessage ?? $"{value} does not exist in {_tableName}.";
            return new ValidationResult(msg);
        }

        return ValidationResult.Success;
    }
}

