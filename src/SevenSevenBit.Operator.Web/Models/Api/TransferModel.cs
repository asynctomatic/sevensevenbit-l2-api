namespace SevenSevenBit.Operator.Web.Models.Api;

using System.ComponentModel.DataAnnotations;
using System.Numerics;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Request model to transfer assets between users.
/// </summary>
public class TransferModel
{
    [ValidGuid]
    [Required(ErrorMessage = "SenderVaultIdRequired")]
    [SwaggerSchema(
        Title = "Sender Vault ID",
        Description = "The unique identifier of the transfer sender vault.",
        Format = "uuid")]
    public Guid? FromUserId { get; set; }

    [ValidGuid]
    [ValidReceiverVaultId]
    [SwaggerSchema(
        Title = "Receiver Vault ID",
        Description = "The unique identifier of the transfer recipient vault.",
        Format = "uuid")]
    [Required(ErrorMessage = "ReceiverVaultIdRequired")]
    public Guid? ToUserId { get; set; }

    [ValidGuid]
    [Required(AllowEmptyStrings = false, ErrorMessage = "AssetIdRequired")]
    [SwaggerSchema(
        Title = "Asset ID",
        Description = "The unique identifier of the asset being transferred.",
        Format = "uuid")]
    public Guid? AssetId { get; set; }

    [ValidHexString]
    [SwaggerSchema(
        Title = "Token ID",
        Description = "The hexadecimal string representation of the token ID, if applicable (ie. ERC-721/ERC-1155).",
        Format = "hex")]
    public string TokenId { get; set; }

    [ValidExpirationTimestamp]
    [Required(ErrorMessage = "ExpirationTimestampRequired")]
    [SwaggerSchema(
        Title = "Expiration Timestamp",
        Description = "The timestamp at which this transfer becomes invalid, in seconds since the Unix epoch.",
        Format = "int64")]
    public long ExpirationTimestamp { get; set; }

    [Range(0, int.MaxValue)]
    [Required(ErrorMessage = "NonceRequired")]
    [SwaggerSchema(
        Title = "Nonce",
        Description = "The unique nonce for the transfer.",
        Format = "int32")]
    public int Nonce { get; set; }

    [ValidAmount]
    [Required(ErrorMessage = "OperationAmountRequired")]
    [SwaggerSchema(
        Title = "Amount",
        Description = "The amount of the asset to be transferred.")]
    public BigInteger Amount { get; set; }

    [Required(ErrorMessage = "StarkSignatureRequired")]
    [SwaggerSchema(
        Title = "Signature",
        Description = "A cryptographic signature of this transfer, using the sender's STARK key.")]
    public SignatureModel Signature { get; set; }
}