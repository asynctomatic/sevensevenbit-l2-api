namespace SevenSevenBit.Operator.Web.Models.Api.Settlement;

using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Request model to transfer assets between users.
/// </summary>
public class SubmitSettlementModel
{
    [Required(ErrorMessage = "OrderARequired")]
    [SwaggerSchema(
        Title = "Order A",
        Description = "The order A details.")]
    public SettlementOrderModel OrderA { get; set; }

    [Required(ErrorMessage = "OrderBRequired")]
    [SwaggerSchema(
        Title = "Order B",
        Description = "The order A details.")]
    public SettlementOrderModel OrderB { get; set; }

    [SwaggerSchema(
        Title = "Settlement Info",
        Description = "The settlement details.")]
#pragma warning disable CS8632
    public SettlementInfoModel? SettlementInfo { get; set; }
#pragma warning restore CS8632
}