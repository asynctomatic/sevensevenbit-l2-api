namespace SevenSevenBit.Operator.Web.Mvc;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Domain.Constants;

[ExcludeFromCodeCoverage]
public class AdminCreatedResult : CreatedResult
{
    public AdminCreatedResult(string location, object value)
        : base($"{Routes.AdminRoutePrefix}{location}", value)
    {
    }
}