namespace SevenSevenBit.Operator.Web.Models.Api;

using System.ComponentModel.DataAnnotations;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

public class AllocateVaultModel
{
    [ValidGuid]
    [SwaggerSchema(
        Title = "Asset ID",
        Description = "The unique identifier of the asset.",
        Format = "uuid")]
    public Guid AssetId { get; set; }

    [ValidGuid]
    [SwaggerSchema(
        Title = "User ID",
        Description = "The unique identifier of the user.",
        Format = "uuid")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "DataAvailabilityRequired")]
    [EnumDataType(typeof(DataAvailabilityModes))]
    [SwaggerSchema(
        Title = "Data Availability Mode",
        Description = "The data availability mode of the vault.",
        Format = "string")]
    public DataAvailabilityModes? DataAvailabilityMode { get; set; }

    [ValidHexString]
    [SwaggerSchema(
        Title = "Token ID",
        Description = "The hexadecimal string representation of the vault's asset token ID, if applicable (ie. ERC-721/ERC-1155).",
        Format = "hex")]
    public string TokenId { get; set; }

    [ValidHexString]
    [SwaggerSchema(
        Title = "Minting Blob",
        Description = "The hexadecimal string representation of the vault's asset minting blob, if applicable (ie. Mintable ERC-20/Mintable ERC-721/Mintable ERC-1155).",
        Format = "hex")]
    public string MintingBlob { get; set; }
}