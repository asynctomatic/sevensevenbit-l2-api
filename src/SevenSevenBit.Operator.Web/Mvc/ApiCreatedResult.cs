namespace SevenSevenBit.Operator.Web.Mvc;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Domain.Constants;

[ExcludeFromCodeCoverage]
public class ApiCreatedResult : CreatedResult
{
    public ApiCreatedResult(string location, object value)
        : base($"{Routes.ApiRoutePrefix}{location}", value)
    {
    }
}