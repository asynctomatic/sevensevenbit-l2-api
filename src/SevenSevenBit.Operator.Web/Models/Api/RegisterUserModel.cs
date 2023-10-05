namespace SevenSevenBit.Operator.Web.Models.Api;

using System.ComponentModel.DataAnnotations;
using Org.BouncyCastle.Math;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using StarkEx.Crypto.SDK.Hashing;
using Swashbuckle.AspNetCore.Annotations;

// TODO These error messages shouldn't be constants as they link directly to the errorCodes..
public record RegisterUserModel
{
    [ValidHexString]
    [Required(AllowEmptyStrings = false, ErrorMessage = "StarkKeyRequired")]
    [SwaggerSchema(
        Title = "STARK Key",
        Description = "The STARK key of the user.",
        Format = "hex")]
    public string StarkKey { get; set; }

    [Required(ErrorMessage = "StarkSignatureRequired")]
    [ValidStarkExSignature(SignableMethods.RegisterUser)]
    [SwaggerSchema(
        Title = "STARK Signature",
        Description = "The ECDSA signature.")]
    public SignatureModel StarkSignature { get; set; }

    public string ToPedersenHash(IPedersenHash pedersenHash)
    {
        // TODO: change message that is signed for registration.
        var hashAsBigInteger = pedersenHash.CreateHash(new BigInteger("77"));

        return hashAsBigInteger.ToString(16);
    }
}