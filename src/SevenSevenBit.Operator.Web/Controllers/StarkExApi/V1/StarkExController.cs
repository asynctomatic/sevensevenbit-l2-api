namespace SevenSevenBit.Operator.Web.Controllers.StarkExApi.V1;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Application.Comparers;
using SevenSevenBit.Operator.Application.DTOs;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Infrastructure.Identity.Auth;
using SevenSevenBit.Operator.Web.Attributes.Routing;
using SevenSevenBit.Operator.Web.Controllers.Api.V1;
using SevenSevenBit.Operator.Web.Models.Api;
using StarkEx.Client.SDK.Enums.Spot;

[ApiController]
[Authorize]
[StarkExRoute("alt-txs")]
[ApiExplorerSettings(GroupName = "starkex-v1")]
[ApiVersion("1.0")]
public class StarkExController : ApiControllerBase
{
    private readonly ILogger<StarkExController> logger;
    private readonly ITransactionService transactionService;
    private readonly IStarkExService starkExService;

    public StarkExController(
        ILogger<StarkExController> logger,
        ITransactionService transactionService,
        IStarkExService starkExService)
    {
        this.logger = logger;
        this.transactionService = transactionService;
        this.starkExService = starkExService;
    }

    [HttpPost]
    [Authorize(Policies.AlternativeTxs)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<StarkExAlternativeTransactionsDto>> IndexAsync(
        [FromBody] StarkExRevertTransactionModel revertTransactionModel,
        CancellationToken cancellationToken)
    {
        // TODO Trigger an alert here or in the StarkEx service
        logger.LogCritical(
            "StarkEx API reverted transaction {TxId} with error code {Code} : {Msg}",
            revertTransactionModel.TxId,
            revertTransactionModel.ReasonCode,
            revertTransactionModel.ReasonMessage);

        var dbTransaction = await transactionService.GetTransactionByStarkExIdAsync(revertTransactionModel.TxId, cancellationToken);

        if (dbTransaction is null)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Transaction not found",
                Detail = $"Transaction number {revertTransactionModel.TxId} was not found in the database",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.TransactionIdNotFound).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogCritical(
                "Transaction number {TxId} was not found in the database",
                revertTransactionModel.TxId);

            return BadRequest(problemDetails);
        }

        var transactionComparer = new TransactionModelComparer();
        if (!transactionComparer.Equals(dbTransaction.RawTransaction, revertTransactionModel.Transaction))
        {
            logger.LogWarning(
                "Transaction number {TxId} doesn't match the transaction StarkEx sent {@Transaction}",
                revertTransactionModel.TxId,
                revertTransactionModel.Transaction);
        }

        if (revertTransactionModel.ReasonCode.Equals(SpotApiCodes.ReplacedBefore) &&
            (dbTransaction.ReplacementTransactions is null || !dbTransaction.ReplacementTransactions.Any()))
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Transaction not replaced",
                Detail = $"Transaction number {revertTransactionModel.TxId} was never replaced before",
                Status = StatusCodes.Status400BadRequest,
                Type = "Undefined",
                Instance = HttpContext.Request.Path,
            };

            logger.LogCritical(
                "Reverted transaction number {TxId} was never replaced before",
                revertTransactionModel.TxId);

            return BadRequest(problemDetails);
        }

        // Transaction was already reverted and replacement tx failed with a stateful error (i.e. != REPLACED_BEFORE)
        // According to the docs, this should never happen
        if (dbTransaction.Status.Equals(TransactionStatus.Reverted) &&
            revertTransactionModel.ReasonCode != SpotApiCodes.ReplacedBefore)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Transaction already reverted",
                Detail = $"Transaction number {revertTransactionModel.TxId} was already reverted",
                Status = StatusCodes.Status400BadRequest,
                Type = "Undefined",
                Instance = HttpContext.Request.Path,
            };

            logger.LogCritical(
                "Transaction number {TxId} was already reverted",
                revertTransactionModel.TxId);

            return BadRequest(problemDetails);
        }

        var altTxs = await starkExService.GetAlternativeTransactionsAsync(
            dbTransaction,
            revertTransactionModel.ReasonCode,
            revertTransactionModel.ReasonMessage,
            cancellationToken);

        return Ok(new StarkExAlternativeTransactionsDto
        {
            AltTxs = altTxs,
        });
    }
}