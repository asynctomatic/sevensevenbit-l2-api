namespace SevenSevenBit.Operator.Web.Models.Api;

using System.ComponentModel.DataAnnotations;
using SevenSevenBit.Operator.Domain.Constants;
using SevenSevenBit.Operator.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Request model to configure the fee model for an operation.
/// </summary>
public class ConfigureFeeModel
{
    [Required(ErrorMessage = "FeeActionRequired")]
    [EnumDataType(typeof(FeeAction))]
    [SwaggerSchema(
        Title = "Operation",
        Description = "The system operation to configure the fee model for.",
        Format = "string")]
    public FeeAction? FeeAction { get; set; }

    [Required(ErrorMessage = "FeeBasisPointsRequired")]
    [Range(FeeConstants.MinBasisPoints, FeeConstants.MaxBasisPoints, ErrorMessage = "FeeBasisPointOutOfRange")]
    [SwaggerSchema(
        Title = "Basis Points",
        Description = "The basis points (1/100 of a percent) of the fee to take on the operation.",
        Format = "int32")]
    public int BasisPoints { get; set; }
}