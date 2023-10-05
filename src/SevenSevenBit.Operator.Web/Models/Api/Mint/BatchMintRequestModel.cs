namespace SevenSevenBit.Operator.Web.Models.Api.Mint;

using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Request model to mint a batch of assets.
/// </summary>
public record BatchMintRequestModel
{
    [Required(ErrorMessage = "MintsRequired")]
    [MinLength(1, ErrorMessage = "MintsRequired")]
    [SwaggerSchema(
        Title = "Mints",
        Description = "The array of assets to mint.",
        Format = "array")]
    public IEnumerable<MintRequestModel> Mints { get; set; }
}