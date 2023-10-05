namespace SevenSevenBit.Operator.Web.Models.Api;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

public class RegisterDetailsModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "UsernameRequired")]
    [StringLength(30, ErrorMessage = "UsernameLengthInvalid", MinimumLength = 6)]
    [FromQuery(Name = "username")]
    [SwaggerSchema(
        Title = "Username",
        Description = "The username of the user.",
        Format = "string")]
    public string Username { get; set; }

    [ValidHexString]
    [Required(AllowEmptyStrings = false, ErrorMessage = "StarkKeyRequired")]
    [FromQuery(Name = "stark_key")]
    [SwaggerSchema(
        Title = "STARK Key",
        Description = "The STARK key of the user.",
        Format = "hex")]
    public string StarkKey { get; set; }

    [ValidEthereumAddress]
    [ValidHexString]
    [Required(AllowEmptyStrings = false, ErrorMessage = "AddressRequired")]
    [FromQuery(Name = "address")]
    [SwaggerSchema(
        Title = "Address",
        Description = "The Ethereum address associated with the user.",
        Format = "hex")]
    public string Address { get; set; }
}