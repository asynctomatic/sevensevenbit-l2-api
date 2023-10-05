namespace SevenSevenBit.Operator.Web.Models.Api;

using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

public class GetVaultsQueryModel : BasePaginatedRequestQueryModel
{
    [FromQuery(Name = "asset_id")]
    [ValidGuid]
    [SwaggerSchema(
        Title = "Asset ID",
        Description = "The unique identifier of the asset.",
        Format = "uuid")]
    public Guid AssetId { get; set; }
}