namespace SevenSevenBit.Operator.Web.Models.Api.Settlement;

using System.Numerics;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Settlement details model.
/// </summary>
public class SettlementInfoModel
{
    [RequiredIf(nameof(OrderAFeeAmount), ErrorCodes.OrderAFeeDestinationVaultIdRequired)]
    [ValidGuid]
    [SwaggerSchema(
        Title = "Order A Fee Destination Vault ID",
        Description = "The unique identifier of the destination vault for the order A fee.",
        Format = "uuid")]
    public Guid? OrderAFeeDestinationVaultId { get; set; }

    [RequiredIf(nameof(OrderAFeeDestinationVaultId), ErrorCodes.OrderAFeeAmountRequired)]
    [SwaggerSchema(
        Title = "Order A Fee Amount",
        Description = "The order A fee amount.",
        Format = "string")]
    public BigInteger? OrderAFeeAmount { get; set; }

    [RequiredIf(nameof(OrderBFeeAmount), ErrorCodes.OrderBFeeDestinationVaultIdRequired)]
    [ValidGuid]
    [SwaggerSchema(
        Title = "Order B Fee Destination Vault ID",
        Description = "The unique identifier of the destination vault for the order B fee.",
        Format = "uuid")]
    public Guid? OrderBFeeDestinationVaultId { get; set; }

    [RequiredIf(nameof(OrderBFeeDestinationVaultId), ErrorCodes.OrderBFeeAmountRequired)]
    [SwaggerSchema(
        Title = "Order B Fee Amount",
        Description = "The order B fee amount.",
        Format = "string")]
    public BigInteger? OrderBFeeAmount { get; set; }
}