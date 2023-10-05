namespace SevenSevenBit.Operator.Web.Attributes.Routing;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Domain.Constants;

[ExcludeFromCodeCoverage]
public class AdminRouteAttribute : RouteAttribute
{
    public AdminRouteAttribute(string template)
        : base($"{Routes.AdminRoutePrefix}{Routes.VersionRoutePrefix}{template}")
    {
    }
}