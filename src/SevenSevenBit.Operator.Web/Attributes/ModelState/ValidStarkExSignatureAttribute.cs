namespace SevenSevenBit.Operator.Web.Attributes.ModelState;

using System.ComponentModel.DataAnnotations;
using SevenSevenBit.Operator.Application.Interfaces.Services.Signatures;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Domain.ValueObjects;
using SevenSevenBit.Operator.Web.Models.Api;
using StarkEx.Crypto.SDK.Hashing;

public class ValidStarkExSignatureAttribute : ValidationAttribute
{
    private readonly SignableMethods signableMethod;

    public ValidStarkExSignatureAttribute(SignableMethods signableMethod)
    {
        this.signableMethod = signableMethod;
    }

    protected override ValidationResult IsValid(
        object value,
        ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        var signature = (SignatureModel)value;

        var model = signableMethod switch
        {
            SignableMethods.RegisterUser => validationContext.ObjectInstance as RegisterUserModel,
            _ => throw new ArgumentOutOfRangeException(nameof(validationContext)),
        };

        var starkExSignatureService = validationContext.GetRequiredService<IStarkExSignatureService>();
        var pedersenHash = validationContext.GetRequiredService<IPedersenHash>();

        var isSignatureValid = starkExSignatureService.ValidateStarkExSignature(
            model!.ToPedersenHash(pedersenHash),
            model!.StarkKey,
            new StarkSignature(signature.R, signature.S));

        return isSignatureValid ?
            ValidationResult.Success :
            new ValidationResult(ErrorCodes.StarkSignatureInvalid.ToString());
    }
}