namespace SevenSevenBit.Operator.Web.Models.Api.Order;

using System.ComponentModel.DataAnnotations;
using System.Numerics;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Web.Attributes.ModelState;

public class SignableOrderRequest
{
    [ValidGuid]
    [Required(ErrorMessage = "UserIdRequired")]
    public Guid UserId { get; set; }

    [EnumDataType(typeof(OrderSide))]
    [Required(ErrorMessage = "OrderSideRequired")]
    public OrderSide Side { get; set; }

    //TODO: [ValidAmount]
    [Required(ErrorMessage = "OperationAmountRequired")]
    public BigInteger BaseAssetAmount { get; set; }

    // TODO: [ValidAmount]
    [Required(ErrorMessage = "OperationAmountRequired")]
    public BigInteger QuoteAssetAmount { get; set; }
}