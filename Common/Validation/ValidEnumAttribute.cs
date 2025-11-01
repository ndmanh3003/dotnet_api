using System.ComponentModel.DataAnnotations;

namespace dotnet.Common.Validation;

public class ValidEnumAttribute<TEnum> : ValidationAttribute where TEnum : struct, Enum
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || Enum.IsDefined(typeof(TEnum), value))
            return ValidationResult.Success;

        return new ValidationResult($"Invalid {typeof(TEnum).Name} value");
    }
}
