namespace SevenSevenBit.Operator.Web.Models.Api.Order;

using System.ComponentModel.DataAnnotations;
using System.Numerics;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Web.Attributes.ModelState;

// TODO: Swagger docs
public class SubmitOrderRequest
{
    [ValidGuid]
    [Required(ErrorMessage = "UserIdRequired")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "OrderSideRequired")]
    [EnumDataType(typeof(OrderSide))]
    public OrderSide Side { get; set; }

    [ValidAmount]
    [Required(ErrorMessage = "OperationAmountRequired")]
    public BigInteger BaseAssetAmountQuantized { get; set; }

    [ValidAmount]
    [Required(ErrorMessage = "OperationAmountRequired")]
    public BigInteger QuoteAssetAmountQuantized { get; set; }

    [Range(0, int.MaxValue)]
    [Required(ErrorMessage = "NonceRequired")]
    public int Nonce { get; set; }

    [ValidExpirationTimestamp]
    [Required(ErrorMessage = "ExpirationTimestampRequired")]
    public long ExpirationTimestamp { get; set; }

    [Required(ErrorMessage = "StarkSignatureRequired")]
    public SignatureModel Signature { get; set; }
}