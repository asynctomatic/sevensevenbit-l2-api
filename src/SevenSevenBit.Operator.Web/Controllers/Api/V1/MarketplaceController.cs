namespace SevenSevenBit.Operator.Web.Controllers.Api.V1;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Application.UseCases.Marketplace.CreateMarketplace;
using SevenSevenBit.Operator.Application.UseCases.Marketplace.Query.GetMarketplace;
using SevenSevenBit.Operator.Application.UseCases.Marketplace.Query.ListMarketplaces;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Infrastructure.Identity.Auth;
using SevenSevenBit.Operator.Web.Attributes.Routing;
using SevenSevenBit.Operator.Web.Models.Api.Marketplace.Request;
using SevenSevenBit.Operator.Web.Models.Api.Marketplace.Response;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Authorize]
[ApiRoute("marketplaces")]
[ApiVersion("1.0")]
public class MarketplaceController : ApiControllerBase
{
    private readonly IMediator mediator;

    public MarketplaceController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [Authorize(Policies.ReadMarketplaces)]
    [SwaggerOperation(
        Summary = "List Marketplaces",
        Description = "This endpoint retrieves paginated a list of marketplaces.",
        OperationId = "ListMarketplaces",
        Tags = new[] { "Marketplace" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a paginated list of marketplaces.", typeof(IEnumerable<MarketplaceResponse>))]
    public async Task<ActionResult<IEnumerable<MarketplaceResponse>>> ListMarketplacesAsync(CancellationToken cancellationToken)
    {
        var query = new ListMarketplacesQuery();
        var result = await mediator.Send(query, cancellationToken);

        return result.Match<ActionResult<IEnumerable<MarketplaceResponse>>>(
            marketplaces => Ok(marketplaces.Select(m => new MarketplaceResponse(m))),
            _ => StatusCode(StatusCodes.Status404NotFound));
    }

    [HttpGet("{marketplaceId:guid:required}")]
    [Authorize(Policies.ReadMarketplaces)]
    [SwaggerOperation(
        Summary = "Get Marketplace",
        Description = "This endpoint retrieves a marketplace by ID.",
        OperationId = "GetMarketplaces",
        Tags = new[] { "Order" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the marketplace.", typeof(MarketplaceResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Marketplace not found.", typeof(ProblemDetails))]
    public async Task<ActionResult<MarketplaceResponse>> GetMarketplaceAsync(
        [FromRoute, SwaggerParameter("The marketplace id.", Required = true)]
        Guid marketplaceId,
        CancellationToken cancellationToken)
    {
        var query = new GetMarketplaceQuery(marketplaceId);
        var result = await mediator.Send(query, cancellationToken);

        return result.Match<ActionResult<MarketplaceResponse>>(
            marketplace => Ok(new MarketplaceResponse(marketplace)),
            _ => StatusCode(StatusCodes.Status404NotFound));
    }

    [HttpPost]
    [Authorize(Policies.WriteMarketplaces)]
    [SwaggerOperation(
        Summary = "Create Marketplace",
        Description = "This endpoint creates a marketplace.",
        OperationId = "CreateMarketplace",
        Tags = new[] { "Marketplace" })]
    [SwaggerResponse(StatusCodes.Status201Created, "Returns the created marketplace.", typeof(MarketplaceResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The marketplace creation request was invalid.", typeof(ProblemDetails))]
    public async Task<ActionResult<MarketplaceResponse>> CreateMarketplaceAsync(
        [FromBody, SwaggerRequestBody("The marketplace creation request.", Required = true)] CreateMarketplaceRequest request,
        CancellationToken cancellationToken)
    {
        // TODO: Move this validation to the command handling pipeline.
        if (request.BaseAssetId == request.QuoteAssetId)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Same base and quote asset.",
                Detail = $"The base asset ID and quote asset ID must be different.",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.SameBaseAndQuoteAssets).ToString(),
                Instance = HttpContext.Request.Path,
            });
        }

        var command = new CreateMarketplaceCommand(
            request.BaseAssetId, request.QuoteAssetId, request.BaseAssetTokenId, request.QuoteAssetTokenId);

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<ActionResult<MarketplaceResponse>>(
            marketplace => ApiCreated($"marketplaces/{marketplace.Id}", new MarketplaceResponse(marketplace)),
            _ => StatusCode(StatusCodes.Status500InternalServerError));
    }
}