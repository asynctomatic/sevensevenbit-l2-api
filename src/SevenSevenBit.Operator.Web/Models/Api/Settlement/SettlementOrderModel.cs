namespace SevenSevenBit.Operator.Web.Models.Api.Settlement;

using System.ComponentModel.DataAnnotations;
using System.Numerics;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

public class SettlementOrderModel
{
    [ValidGuid]
    [Required(ErrorMessage = "BuyVaultIdRequired")]
    [SwaggerSchema(
        Title = "Buy Vault ID",
        Description = "The unique identifier of the buy vault.",
        Format = "uuid")]
    public Guid BuyVaultId { get; set; }

    [ValidAmount]
    [Required(ErrorMessage = "BuyAmountRequired")]
    [SwaggerSchema(
        Title = "Buy Quantized Amount",
        Description = "The amount of the asset to be settled, in quantized form.")]
    public BigInteger BuyQuantizedAmount { get; set; }

    [ValidGuid]
    [Required(ErrorMessage = "SellVaultIdRequired")]
    [SwaggerSchema(
        Title = "Sell Vault ID",
        Description = "The unique identifier of the sell vault.",
        Format = "uuid")]
    public Guid SellVaultId { get; set; }

    [ValidAmount]
    [Required(ErrorMessage = "SellAmountRequired")]
    [SwaggerSchema(
        Title = "Sell Quantized Amount",
        Description = "The amount of the asset to be settled, in quantized form.")]
    public BigInteger SellQuantizedAmount { get; set; }

    [RequiredIf(nameof(FeeQuantizedAmount), ErrorCodes.FeeVaultIdRequired)]
    [ValidGuid]
    [SwaggerSchema(
        Title = "Fee Vault ID",
        Description = "The unique identifier of the transfer sender vault.",
        Format = "uuid")]
    public Guid? FeeVaultId { get; set; }

    [RequiredIf(nameof(FeeVaultId), ErrorCodes.FeeQuantizedAmountRequired)]
    [ValidAmount]
    [SwaggerSchema(
        Title = "Fee Quantized Amount",
        Description = "The amount of the fee asset to be collected, in quantized form.")]
    public BigInteger? FeeQuantizedAmount { get; set; }

    [Required(ErrorMessage = "NonceRequired")]
    [Range(0, int.MaxValue)]
    [SwaggerSchema(
        Title = "Nonce",
        Description = "The unique nonce for the transfer.",
        Format = "int32")]
    public int Nonce { get; set; }

    [Required(ErrorMessage = "StarkSignatureRequired")]
    [SwaggerSchema(
        Title = "Signature",
        Description = "A cryptographic signature of this transfer, using the sender's STARK key.")]
    public SignatureModel Signature { get; set; }

    [ValidExpirationTimestamp]
    [Required(ErrorMessage = "ExpirationTimestampRequired")]
    [SwaggerSchema(
        Title = "Expiration Timestamp",
        Description = "The timestamp at which this transfer becomes invalid, in seconds since the Unix epoch.",
        Format = "int64")]
    public long ExpirationTimestamp { get; set; }
}