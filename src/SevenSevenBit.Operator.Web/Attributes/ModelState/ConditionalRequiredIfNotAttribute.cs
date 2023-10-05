namespace SevenSevenBit.Operator.Web.Attributes.ModelState;

using System.ComponentModel.DataAnnotations;
using SevenSevenBit.Operator.Domain.Enums;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ConditionalRequiredIfNotAttribute : ValidationAttribute
{
    private readonly string conditionalPropertyName;
    private readonly object conditionalPropertyValue;
    private readonly ErrorCodes errorMessage;

    public ConditionalRequiredIfNotAttribute(string conditionalPropertyName, object conditionalPropertyValue, ErrorCodes errorMessage)
    {
        this.conditionalPropertyName = conditionalPropertyName;
        this.conditionalPropertyValue = conditionalPropertyValue;
        this.errorMessage = errorMessage;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var currentConditionalProperty = validationContext?.ObjectType.GetProperty(conditionalPropertyName);

        if (currentConditionalProperty == null)
        {
            throw new InvalidOperationException($"The Field '{conditionalPropertyName}' doesn't exist.");
        }

        var currentConditionalPropertyValue = currentConditionalProperty.GetValue(validationContext.ObjectInstance, null);

        return currentConditionalPropertyValue?.Equals(conditionalPropertyValue) == false && string.IsNullOrEmpty(value?.ToString())
            ? new ValidationResult(errorMessage.ToString())
            : ValidationResult.Success;
    }
}