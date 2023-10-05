namespace SevenSevenBit.Operator.Web.Models.Api;

using System.ComponentModel.DataAnnotations;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Model representing an ECDSA signature data.
/// </summary>
public record SignatureModel
{
    [ValidHexString]
    [Required(AllowEmptyStrings = false, ErrorMessage = "StarkSignatureRRequired")]
    [SwaggerSchema(
        Title = "Signature R",
        Description = "The R component of the ECDSA signature, represented as a hexadecimal string.",
        Format = "hex")]
    public string R { get; set; }

    [ValidHexString]
    [Required(AllowEmptyStrings = false, ErrorMessage = "StarkSignatureSRequired")]
    [SwaggerSchema(
        Title = "Signature S",
        Description = "The S component of the ECDSA signature, represented as a hexadecimal string.",
        Format = "hex")]
    public string S { get; set; }
}