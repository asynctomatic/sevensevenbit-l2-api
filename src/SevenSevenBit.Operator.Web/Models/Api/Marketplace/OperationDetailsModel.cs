namespace SevenSevenBit.Operator.Web.Models.Api.Marketplace;

using System.Numerics;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

public class OperationDetailsModel
{
    [SwaggerSchema(
        Title = "Operation Type",
        Description = "The type of the operation.",
        Format = "string")]
    public OperationType OperationType { get; set; }

    [RequiredIf(nameof(OperationType), Marketplace.OperationType.Sell, ErrorCodes.SellerIdRequired)]
    [ValidGuid]
    [SwaggerSchema(
        Title = "Seller ID",
        Description = "The unique identifier of the user who is placing the sell offer.",
        Format = "uuid")]
    public Guid? SellerId { get; set; }

    [RequiredIf(nameof(OperationType), Marketplace.OperationType.Sell, ErrorCodes.AssetIdRequired)]
    [ValidGuid]
    [SwaggerSchema(
        Title = "Asset ID",
        Description = "The unique identifier of the asset that is being sold.",
        Format = "uuid")]
    public Guid? AssetId { get; set; }

    // This amount is unquantized
    [RequiredIf(nameof(OperationType), Marketplace.OperationType.Sell, ErrorCodes.QuantityRequired)]
    [ValidAmount]
    [SwaggerSchema(
        Title = "Quantity",
        Description = "The quantity of the product that is being sold.")]
    public BigInteger? Quantity { get; set; }

    [ValidHexString]
    [SwaggerSchema(
        Title = "Token ID",
        Description = "The hexadecimal string representation of the token ID, if applicable (ie. ERC-721/ERC-1155).",
        Format = "hex")]
    public string TokenId { get; set; }

    [RequiredIf(nameof(OperationType), Marketplace.OperationType.Sell, ErrorCodes.CurrencyIdRequired)]
    [ValidGuid]
    [SwaggerSchema(
        Title = "Currency ID",
        Description = "The unique identifier of the asset that is being used as currency.",
        Format = "uuid")]
    public Guid? CurrencyId { get; set; }

    // This amount is unquantized
    [RequiredIf(nameof(OperationType), Marketplace.OperationType.Sell, ErrorCodes.PriceRequired)]
    [ValidAmount]
    [SwaggerSchema(
        Title = "Price",
        Description = "The price of the asset that is being sold.")]
    public BigInteger? Price { get; set; }

    [RequiredIf(nameof(OperationType), Marketplace.OperationType.Buy, ErrorCodes.BuyerIdRequired)]
    [ValidGuid]
    [SwaggerSchema(
        Title = "Buyer ID",
        Description = "The unique identifier of the user who is placing the buy order.",
        Format = "uuid")]
    public Guid? BuyerId { get; set; }

    [RequiredIf(nameof(OperationType), Marketplace.OperationType.Buy, ErrorCodes.OfferIdRequired)]
    [ValidGuid]
    [SwaggerSchema(
        Title = "Offer ID",
        Description = "The unique identifier of the offer that is being bought.",
        Format = "uuid")]
    public Guid? OfferId { get; set; }
}