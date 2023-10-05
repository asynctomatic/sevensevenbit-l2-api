namespace SevenSevenBit.Operator.Web.Models.Api;

using Microsoft.AspNetCore.Mvc;
using NodaTime;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

public class GetUsersQueryModel : BasePaginatedRequestQueryModel
{
    [FromQuery(Name = "creation_date")]
    [SwaggerSchema(
        Title = "Creation Date",
        Description = "The creation date of the user.",
        Format = "string")]
    public LocalDateTime? CreationDate { get; set; }

    [RequiredIf(nameof(CreationDate), ErrorCodes.FilterIsRequired)]
    [FromQuery(Name = "creation_date_comparison")]
    [SwaggerSchema(
        Title = "Creation Date Filter",
        Description = "The filter option used for creation date.",
        Format = "string")]
    public FilterOptions? CreationDateFilterOption { get; set; }
}