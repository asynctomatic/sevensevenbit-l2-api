namespace SevenSevenBit.Operator.Web.Models.Admin;

using System.ComponentModel.DataAnnotations;
using SevenSevenBit.Operator.Web.Attributes.ModelState;

public class RegisterStarkExInstanceModel
{
    [ValidHexString]
    [ValidEthereumAddress]
    [Required(AllowEmptyStrings = false, ErrorMessage = "StarkExAddressRequired")]
    public string StarkExAddress { get; set; }

    [ValidHexString]
    [Required(AllowEmptyStrings = false, ErrorMessage = "StarkExContractTokensAdminKeyRequired")]
    public string StarkExContractTokensAdminKey { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "StarkExInstanceNameRequired")]
    public string StarkExInstanceName { get; set; }

    [Url(ErrorMessage = "StarkExHostnameInvalid")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "StarkExHostnameRequired")]
    public string StarkExHostname { get; set; }

    [ValidStarkExVersion]
    [Required(AllowEmptyStrings = false, ErrorMessage = "StarkExVersionRequired")]
    public string StarkExVersion { get; set; }

    [ValidValidiumTreeHeight]
    [Required(AllowEmptyStrings = false, ErrorMessage = "ValidiumTreeHeightRequired")]
    public int? ValidiumTreeHeight { get; set; }

    [ValidZkRollupTreeHeight]
    [Required(AllowEmptyStrings = false, ErrorMessage = "ZkRollupTreeHeightRequired")]
    public int? ZkRollupTreeHeight { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "IsUniqueMintingEnabledRequired")]
    public bool? IsUniqueMintingEnabled { get; set; }

    [Required(ErrorMessage = "StarkExDeploymentBlockMissing")]
    [Range(0, long.MaxValue, ErrorMessage = "StarkExDeploymentBlockOutOfRange")]
    public long DeploymentBlock { get; set; }
}