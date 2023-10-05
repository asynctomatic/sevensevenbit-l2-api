namespace SevenSevenBit.Operator.Web.Controllers.Api.V1;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Application.UseCases.Marketplace.CancelOrder;
using SevenSevenBit.Operator.Application.UseCases.Marketplace.GetSignableOrder;
using SevenSevenBit.Operator.Application.UseCases.Marketplace.Query.GetMarketplaceOrder;
using SevenSevenBit.Operator.Application.UseCases.Marketplace.Query.ListMarketplaceOrders;
using SevenSevenBit.Operator.Application.UseCases.Marketplace.SubmitOrder;
using SevenSevenBit.Operator.Domain.ValueObjects;
using SevenSevenBit.Operator.Infrastructure.Identity.Auth;
using SevenSevenBit.Operator.Web.Attributes.Routing;
using SevenSevenBit.Operator.Web.Models.Api.Marketplace.Response;
using SevenSevenBit.Operator.Web.Models.Api.Order;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Authorize]
[ApiRoute("marketplaces")]
[ApiVersion("1.0")]
public class OrderController : ApiControllerBase
{
    private readonly IMediator mediator;

    public OrderController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("{marketplaceId:guid:required}/orders")]
    [Authorize(Policies.ReadMarketplaces)]
    [SwaggerOperation(
        Summary = "List Marketplace Orders",
        Description = "This endpoint retrieves paginated a list of marketplace orders.",
        OperationId = "ListMarketplaceOrders",
        Tags = new[] { "Order" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a paginated list of marketplace orders.", typeof(IEnumerable<OrderResponse>))]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> ListMarketplaceOrdersAsync(
        [FromRoute, SwaggerParameter("The marketplace id.", Required = true)] Guid marketplaceId,
        [FromQuery, SwaggerRequestBody("The buy orders query model.", Required = true)]
        ListMarketplaceOrdersRequest request,
        CancellationToken cancellationToken)
    {
        var query = new ListMarketplaceOrdersQuery(marketplaceId, request.Side, request.IncludeInactive);
        var result = await mediator.Send(query, cancellationToken);

        return result.Match<ActionResult<IEnumerable<OrderResponse>>>(
            orders => Ok(orders.Select(o => new OrderResponse(o))),
            _ => StatusCode(StatusCodes.Status404NotFound));
    }

    [HttpGet("{marketplaceId:guid:required}/orders/{orderId:guid:required}")]
    [Authorize(Policies.ReadOrders)]
    [SwaggerOperation(
        Summary = "Get Marketplace Order",
        Description = "This endpoint retrieves a marketplace order by ID.",
        OperationId = "GetMarketplaceOrder",
        Tags = new[] { "Order" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the marketplace order.", typeof(OrderResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Order not found.", typeof(ProblemDetails))]
    public async Task<ActionResult<OrderResponse>> GetMarketplaceOrderAsync(
        [FromRoute, SwaggerParameter("The marketplace id.", Required = true)]
        Guid marketplaceId,
        [FromRoute, SwaggerParameter("The order id.", Required = true)]
        Guid orderId,
        CancellationToken cancellationToken)
    {
        var query = new GetMarketplaceOrderQuery(marketplaceId, orderId);
        var result = await mediator.Send(query, cancellationToken);

        return result.Match<ActionResult<OrderResponse>>(
            order => Ok(new OrderResponse(order)),
            _ => StatusCode(StatusCodes.Status404NotFound));
    }

    [HttpPost("{marketplaceId:guid:required}/orders")]
    [Authorize(Policies.WriteOrders)]
    [SwaggerOperation(
        Summary = "Submit Order",
        Description = "This endpoint submits a marketplace order.",
        OperationId = "SubmitOrder",
        Tags = new[] { "Order" })]
    [SwaggerResponse(StatusCodes.Status201Created, "Returns the submitted marketplace order.", typeof(OrderResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The order submission request was invalid.", typeof(ProblemDetails))]
    public async Task<ActionResult<OrderResponse>> SubmitOrderAsync(
        [FromRoute, SwaggerParameter("The marketplace id.", Required = true)]
        Guid marketplaceId,
        [FromBody, SwaggerRequestBody("The order submission request.", Required = true)]
        SubmitOrderRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SubmitOrderCommand(
            marketplaceId,
            request.UserId,
            request.Side,
            request.BaseAssetAmountQuantized,
            request.QuoteAssetAmountQuantized,
            request.Nonce,
            request.ExpirationTimestamp,
            new StarkSignature(request.Signature.R, request.Signature.S));

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<ActionResult<OrderResponse>>(
            order => ApiCreated($"marketplaces/{marketplaceId}/orders/{order.Id}", new OrderResponse(order)),
            _ => StatusCode(StatusCodes.Status500InternalServerError));
    }

    [HttpPost("{marketplaceId:guid:required}/orders/signable")]
    [Authorize(Policies.WriteOrders)]
    [SwaggerOperation(
        Summary = "Get Signable Order",
        Description = "This endpoint allows for fetching signable orders.",
        OperationId = "GetSignableOrder",
        Tags = new[] { "Order" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the signable order.", typeof(SignableOrderResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The signable order request was invalid.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found.", typeof(ProblemDetails))]
    public async Task<ActionResult<SignableOrderResponse>> GetSignableOrderAsync(
        [FromRoute, SwaggerParameter("The marketplace id.", Required = true)]
        Guid marketplaceId,
        [FromBody, SwaggerRequestBody("The signable order request.", Required = true)]
        SignableOrderRequest request,
        CancellationToken cancellationToken)
    {
        var command = new GetSignableOrderCommand(
            marketplaceId, request.UserId, request.Side, request.BaseAssetAmount, request.QuoteAssetAmount);

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<ActionResult<SignableOrderResponse>>(
            signableOrder => Ok(new SignableOrderResponse(signableOrder)),
            _ => StatusCode(StatusCodes.Status500InternalServerError));
    }

    [HttpDelete("{marketplaceId:guid:required}/orders/{orderId:guid:required}")]
    [Authorize(Policies.WriteOrders)]
    [SwaggerOperation(
        Summary = "Cancel Order",
        Description = "This endpoint cancels a marketplace order.",
        OperationId = "CancelOrder",
        Tags = new[] { "Order" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Order cancelled successfully.", typeof(OrderResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid cancel request.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Order not found.", typeof(ProblemDetails))]
    public async Task<ActionResult<OrderResponse>> CancelOrderAsync(
        [FromRoute, SwaggerParameter("The marketplace id.", Required = true)]
        Guid marketplaceId,
        [FromRoute, SwaggerParameter("The order id.", Required = true)]
        Guid orderId,
        CancellationToken cancellationToken)
    {
        var command = new CancelOrderCommand(marketplaceId, orderId);
        var result = await mediator.Send(command, cancellationToken);

        return result.Match<ActionResult<OrderResponse>>(
            order => Ok(new OrderResponse(order)),
            _ => StatusCode(StatusCodes.Status500InternalServerError));
    }
}