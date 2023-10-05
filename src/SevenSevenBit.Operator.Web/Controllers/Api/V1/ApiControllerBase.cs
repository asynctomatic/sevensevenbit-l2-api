namespace SevenSevenBit.Operator.Web.Controllers.Api.V1;

using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SevenSevenBit.Operator.Web.Mvc;

[ExcludeFromCodeCoverage]
[ApiExplorerSettings(GroupName = "api-v1")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public abstract class ApiControllerBase : ControllerBase
{
    [NonAction]
    public virtual ApiCreatedResult ApiCreated(string uri, [ActionResultObjectValue] object value)
    {
        if (uri == null)
        {
            throw new ArgumentNullException(nameof(uri));
        }

        return new ApiCreatedResult(uri, value);
    }
}