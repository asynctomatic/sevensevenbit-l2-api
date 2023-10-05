namespace SevenSevenBit.Operator.Web.Controllers.Admin.V1;

using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

[ExcludeFromCodeCoverage]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public abstract class AdminControllerBase : ControllerBase
{
}