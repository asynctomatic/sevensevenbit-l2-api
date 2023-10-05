namespace SevenSevenBit.Operator.Web.Controllers.Api.V1;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Application.DTOs.Entities;
using SevenSevenBit.Operator.Application.DTOs.Pagination;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Infrastructure.Identity.Auth;
using SevenSevenBit.Operator.Web.Attributes.Routing;
using SevenSevenBit.Operator.Web.Models.Api;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Authorize]
[ApiRoute("transactions")]
[ApiVersion("1.0")]
public class TransactionController : ApiControllerBase
{
    private readonly ITransactionService transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        this.transactionService = transactionService;
    }

    [HttpGet]
    [Authorize(Policies.ReadTransactions)]
    [SwaggerOperation(
        Summary = "Get All Transactions",
        Description = "This endpoint fetches all transactions submitted by the system, with support for filters and pagination.",
        OperationId = "GetAllTransactions",
        Tags = new[] { "Transaction" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns all transactions submitted by the system (paginated).", typeof(PaginatedResponseDto<TransactionDto>))]
    public async Task<ActionResult<PaginatedResponseDto<TransactionDto>>> IndexAsync(
        [FromQuery, SwaggerParameter("The pagination and filters for the transaction request.", Required = true)] GetTransactionsQueryModel queryModel,
        CancellationToken cancellationToken)
    {
        var transactions = await transactionService.GetTransactionsAsync(
            new(queryModel.PageNumber, queryModel.PageSize),
            queryModel.StarkExTransactionId,
            queryModel.StarkExTransactionFilterOption,
            queryModel.TransactionStatus,
            queryModel.TransactionStatusFilterOption,
            queryModel.TransactionType,
            queryModel.TransactionTypeFilterOption,
            queryModel.SortBy,
            cancellationToken);

        return Ok(transactions);
    }

    [HttpGet("{transactionId:required:guid}")]
    [Authorize(Policies.ReadTransactions)]
    [SwaggerOperation(
        Summary = "Get Transaction",
        Description = "This endpoint fetches a specific transaction by ID.",
        OperationId = "GetTransaction",
        Tags = new[] { "Transaction" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a transaction.", typeof(TransactionDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found.", typeof(ProblemDetails))]
    public async Task<ActionResult<TransactionDto>> IndexAsync(
        [FromRoute, SwaggerParameter("The transaction ID.", Required = true)] Guid transactionId,
        CancellationToken cancellationToken)
    {
        var transaction = await transactionService.GetTransactionAsync(transactionId, cancellationToken);

        if (transaction is null)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Transaction Not Found",
                Detail = $"Transaction {transactionId} not found.",
                Status = StatusCodes.Status404NotFound,
                Type = ((int)ErrorCodes.TransactionIdNotFound).ToString(),
                Instance = HttpContext.Request.Path,
            };

            return NotFound(problemDetails);
        }

        return Ok(new TransactionDto(transaction));
    }
}