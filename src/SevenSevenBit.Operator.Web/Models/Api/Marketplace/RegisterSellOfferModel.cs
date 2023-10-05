namespace SevenSevenBit.Operator.Web.Models.Api.Marketplace;

using System.ComponentModel.DataAnnotations;
using System.Numerics;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

public class RegisterSellOfferModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "SellerIdRequired")]
    [ValidGuid]
    [SwaggerSchema(
        Title = "Seller ID",
        Description = "The unique identifier of the user who is placing the sell offer.",
        Format = "uuid")]
    public Guid? SellerId { get; set; }

    [ValidGuid]
    [Required(ErrorMessage = "ProductVaultIdRequired")]
    [SwaggerSchema(
        Title = "Product Vault ID",
        Description = "The unique identifier of the product vault.",
        Format = "uuid")]
    public Guid? ProductVaultId { get; set; }

    [ValidAmount]
    [Required(ErrorMessage = "ProductAmountRequired")]
    [SwaggerSchema(
        Title = "Product Amount",
        Description = "The quantized amount of the product being sold.")]
    public BigInteger? ProductAmount { get; set; }

    [ValidGuid]
    [SwaggerSchema(
        Title = "Currency Vault ID",
        Description = "The unique identifier of the currency vault.",
        Format = "uuid")]
    [Required(ErrorMessage = "CurrencyVaultIdRequired")]
    public Guid? CurrencyVaultId { get; set; }

    [ValidAmount]
    [Required(ErrorMessage = "CurrencyAmountRequired")]
    [SwaggerSchema(
        Title = "Currency Amount",
        Description = "The quantized amount of the currency for which the product is being sold.")]
    public BigInteger? CurrencyAmount { get; set; }

    [ValidExpirationTimestamp]
    [Required(ErrorMessage = "ExpirationTimestampRequired")]
    [SwaggerSchema(
        Title = "Expiration Timestamp",
        Description = "The timestamp at which this settlement becomes invalid, in seconds since the Unix epoch.",
        Format = "int64")]
    public long? ExpirationTimestamp { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "InvalidNonce")]
    [Required(ErrorMessage = "NonceRequired")]
    [SwaggerSchema(
        Title = "Nonce",
        Description = "The unique nonce for the transfer.",
        Format = "int32")]
    public int? Nonce { get; set; }

    [MaxLength(100, ErrorMessage = "ProductNameTooLong")]
    [SwaggerSchema(
        Title = "Product name",
        Description = "The product name if the product hasn't been listed before in the marketplace.",
        Format = "string")]
    public string ProductName { get; set; }

    [MaxLength(300, ErrorMessage = "ProductDescriptionTooLong")]
    [SwaggerSchema(
        Title = "Product description",
        Description = "The product description if the product hasn't been listed before in the marketplace.",
        Format = "string")]
    public string ProductDescription { get; set; }

    [Required(ErrorMessage = "StarkSignatureRequired")]
    [SwaggerSchema(
        Title = "Signature",
        Description = "A cryptographic signature of this order, using the seller's STARK key.")]
    public SignatureModel Signature { get; set; }
}