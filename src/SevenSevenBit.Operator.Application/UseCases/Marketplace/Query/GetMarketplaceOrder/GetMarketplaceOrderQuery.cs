namespace SevenSevenBit.Operator.Application.UseCases.Marketplace.Query.GetMarketplaceOrder;

using LanguageExt.Common;
using MediatR;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;

public record GetMarketplaceOrderQuery(Guid MarketplaceId, Guid OrderId) : IRequest<Result<MarketplaceOrder>>;