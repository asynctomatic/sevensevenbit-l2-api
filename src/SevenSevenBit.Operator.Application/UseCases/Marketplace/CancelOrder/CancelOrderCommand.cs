namespace SevenSevenBit.Operator.Application.UseCases.Marketplace.CancelOrder;

using LanguageExt.Common;
using MediatR;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;

public record CancelOrderCommand(Guid MarketplaceId, Guid OrderId)
    : IRequest<Result<MarketplaceOrder>>;