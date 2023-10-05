namespace SevenSevenBit.Operator.Web.Attributes.ModelState;

using System.ComponentModel.DataAnnotations;
using SevenSevenBit.Operator.Domain.Enums;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class RequiredIfAttribute : ValidationAttribute
{
    private readonly string requiredProperty;
    private readonly object conditionValue;
    private readonly ErrorCodes errorMessage;

    public RequiredIfAttribute(string requiredProperty, ErrorCodes errorMessage)
    {
        this.requiredProperty = requiredProperty;
        this.errorMessage = errorMessage;
    }

    public RequiredIfAttribute(string requiredProperty, object conditionValue, ErrorCodes errorMessage)
    {
        this.requiredProperty = requiredProperty;
        this.errorMessage = errorMessage;
        this.conditionValue = conditionValue;
    }

    protected override ValidationResult IsValid(
        object value,
        ValidationContext validationContext)
    {
        if (requiredProperty is null)
        {
            return ValidationResult.Success;
        }

        var property = validationContext.ObjectType.GetProperty(requiredProperty);
        if (property is null)
        {
            throw new InvalidOperationException($"The Field '{requiredProperty}' doesn't exist.");
        }

        var requiredPropertyValue = property.GetValue(validationContext.ObjectInstance);
        if ((requiredPropertyValue is not null && value is null) ||
            (conditionValue is not null && requiredPropertyValue?.ToString() == conditionValue.ToString() && value is null))
        {
            return new ValidationResult(errorMessage.ToString());
        }

        return ValidationResult.Success;
    }
}