namespace SevenSevenBit.Operator.Web.Attributes.Routing;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Domain.Constants;

[ExcludeFromCodeCoverage]
public class ApiRouteAttribute : RouteAttribute
{
    public ApiRouteAttribute(string template)
        : base($"{Routes.ApiRoutePrefix}{Routes.VersionRoutePrefix}{template}")
    {
    }
}