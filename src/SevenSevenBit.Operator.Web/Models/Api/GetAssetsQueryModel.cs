namespace SevenSevenBit.Operator.Web.Models.Api;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using StarkEx.Crypto.SDK.Enums;
using Swashbuckle.AspNetCore.Annotations;

public class GetAssetsQueryModel : BasePaginatedRequestQueryModel
{
    [EnumDataType(typeof(AssetType))]
    [FromQuery(Name = "asset_type")]
    [SwaggerSchema(
        Title = "Asset Type",
        Description = "The type of the asset.",
        Format = "string")]
    public AssetType? AssetType { get; set; }
}