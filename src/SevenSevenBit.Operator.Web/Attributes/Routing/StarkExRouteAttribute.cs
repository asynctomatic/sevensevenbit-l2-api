namespace SevenSevenBit.Operator.Web.Attributes.Routing;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Domain.Constants;

[ExcludeFromCodeCoverage]
public class StarkExRouteAttribute : RouteAttribute
{
    public StarkExRouteAttribute(string template)
        : base($"{Routes.StarkExRoutePrefix}{Routes.VersionRoutePrefix}{template}")
    {
    }
}