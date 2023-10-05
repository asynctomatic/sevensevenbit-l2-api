namespace SevenSevenBit.Operator.Web.Models.Admin;

using System.ComponentModel.DataAnnotations;
using SevenSevenBit.Operator.Web.Attributes.ModelState;

public class RegisterTenantModel
{
    [Required(AllowEmptyStrings = false)]
    public Guid StarkExInstanceId { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }

    [Required(AllowEmptyStrings = false)]
    public int ChainId { get; set; }

    [ValidHexString]
    [ValidEthereumAddress]
    [Required(AllowEmptyStrings = false)]
    public string VerifyingContractAddress { get; set; }

    [Required(AllowEmptyStrings = false)]
    public int Version { get; set; }
}