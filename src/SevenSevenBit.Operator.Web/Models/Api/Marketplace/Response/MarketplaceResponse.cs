namespace SevenSevenBit.Operator.Web.Models.Api.Marketplace.Response;

using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;
using Swashbuckle.AspNetCore.Annotations;

[ExcludeFromCodeCoverage]
public class MarketplaceResponse
{
    public MarketplaceResponse()
    {
    }

    [JsonConstructor]
    public MarketplaceResponse(
        Guid id,
        Guid baseAssetId,
        Guid quoteAssetId,
        string baseAssetTokenId,
        string quoteAssetTokenId)
    {
        Id = id;
        BaseAssetId = baseAssetId;
        QuoteAssetId = quoteAssetId;
        BaseAssetTokenId = baseAssetTokenId;
        QuoteAssetTokenId = quoteAssetTokenId;
    }

    public MarketplaceResponse(Marketplace marketplace)
    {
        Id = marketplace.Id;
        BaseAssetId = marketplace.BaseAssetId;
        QuoteAssetId = marketplace.QuoteAssetId;
        BaseAssetTokenId = marketplace.BaseAssetTokenId;
        QuoteAssetTokenId = marketplace.QuoteAssetTokenId;
    }

    [SwaggerSchema(
        Title = "Marketplace ID",
        Description = "The unique ID of the marketplace.",
        Format = "uuid")]
    public Guid Id { get; set; }

    [SwaggerSchema(
        Title = "Base Asset ID",
        Description = "The ID of the marketplace base asset.",
        Format = "uuid")]
    public Guid BaseAssetId { get; set; }

    [SwaggerSchema(
        Title = "Quote Asset ID",
        Description = "The ID of the marketplace quote asset.",
        Format = "uuid")]
    public Guid QuoteAssetId { get; set; }

    [SwaggerSchema(
        Title = "Base Asset Token ID",
        Description = "The token ID of the marketplace base asset (for ERC721 and ERC1155 assets).",
        Format = "hex")]
    public string BaseAssetTokenId { get; set; }

    [SwaggerSchema(
        Title = "Quote Asset Token ID",
        Description = "The token ID of the marketplace base asset (for ERC721 and ERC1155 assets).",
        Format = "hex")]
    public string QuoteAssetTokenId { get; set; }

    public static implicit operator MarketplaceResponse(Marketplace marketplace) => new(marketplace);
}