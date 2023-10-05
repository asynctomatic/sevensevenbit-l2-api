namespace SevenSevenBit.Operator.Web.Models.Api.Marketplace;

using System.ComponentModel.DataAnnotations;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

public class RegisterBuyOrderModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "OfferIdRequired")]
    [ValidGuid]
    [SwaggerSchema(
        Title = "Offer ID",
        Description = "The unique identifier of the marketplace offer.",
        Format = "uuid")]
    public Guid? OfferId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "BuyerIdRequired")]
    [ValidGuid]
    [SwaggerSchema(
        Title = "Buyer ID",
        Description = "The unique identifier of the user who is placing the buy order.",
        Format = "uuid")]
    public Guid? BuyerId { get; set; }

    [ValidGuid]
    [Required(ErrorMessage = "ProductVaultIdRequired")]
    [SwaggerSchema(
        Title = "Product Vault ID",
        Description = "The unique identifier of the product vault.",
        Format = "uuid")]
    public Guid? ProductVaultId { get; set; }

    [ValidGuid]
    [SwaggerSchema(
        Title = "Currency Vault ID",
        Description = "The unique identifier of the currency vault.",
        Format = "uuid")]
    [Required(ErrorMessage = "CurrencyVaultIdRequired")]
    public Guid? CurrencyVaultId { get; set; }

    [ValidExpirationTimestamp]
    [Required(ErrorMessage = "ExpirationTimestampRequired")]
    [SwaggerSchema(
        Title = "Expiration Timestamp",
        Description = "The timestamp at which this transfer becomes invalid, in seconds since the Unix epoch.",
        Format = "int64")]
    public long? ExpirationTimestamp { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "InvalidNonce")]
    [Required(ErrorMessage = "NonceRequired")]
    [SwaggerSchema(
        Title = "Nonce",
        Description = "The unique nonce for the settlement.",
        Format = "int32")]
    public int? Nonce { get; set; }

    [Required(ErrorMessage = "StarkSignatureRequired")]
    [SwaggerSchema(
        Title = "Signature",
        Description = "A cryptographic signature of this order, using the buyer's STARK key.")]
    public SignatureModel Signature { get; set; }
}