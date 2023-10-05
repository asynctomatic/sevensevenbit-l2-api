namespace SevenSevenBit.Operator.Web.Models.Api;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

public class GetTransactionsQueryModel : BasePaginatedRequestQueryModel
{
    [EnumDataType(typeof(TransactionStatus))]
    [FromQuery(Name = "transaction_status")]
    [SwaggerSchema(
        Title = "Transaction Status",
        Description = "The status of the transaction.",
        Format = "string")]
    public TransactionStatus? TransactionStatus { get; set; }

    [RequiredIf(nameof(TransactionStatus), ErrorCodes.FilterIsRequired)]
    [FromQuery(Name = "transaction_status_comparison")]
    [SwaggerSchema(
        Title = "Transaction Status Filter",
        Description = "The filter option used for transaction status.",
        Format = "string")]
    public FilterOptions? TransactionStatusFilterOption { get; set; }

    [FromQuery(Name = "starkex_tx_id")]
    [SwaggerSchema(
        Title = "StarkEx Transaction ID",
        Description = "The unique identifier of the StarkEx transaction.",
        Format = "integer")]
    public long? StarkExTransactionId { get; set; }

    [RequiredIf(nameof(StarkExTransactionId), ErrorCodes.FilterIsRequired)]
    [FromQuery(Name = "starkex_tx_id_comparison")]
    [SwaggerSchema(
        Title = "StarkEx Transaction Filter",
        Description = "The filter option used for the StarkEx transaction ID.",
        Format = "string")]
    public FilterOptions? StarkExTransactionFilterOption { get; set; }

    [FromQuery(Name = "tx_type")]
    [SwaggerSchema(
        Title = "Transaction Type",
        Description = "Specifies the type of transaction.",
        Format = "string")]
    public StarkExOperation? TransactionType { get; set; }

    [RequiredIf(nameof(TransactionType), ErrorCodes.FilterIsRequired)]
    [FromQuery(Name = "tx_type_comparison")]
    [SwaggerSchema(
        Title = "StarkEx Transaction Filter",
        Description = "The filter option used for the transaction type.",
        Format = "string")]
    public FilterOptions? TransactionTypeFilterOption { get; set; }
}